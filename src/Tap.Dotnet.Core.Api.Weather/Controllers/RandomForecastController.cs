using Microsoft.AspNetCore.Mvc;
using Tap.Dotnet.Core.Api.Weather.Interfaces;
using Tap.Dotnet.Core.Api.Weather.Models;

namespace Tap.Dotnet.Core.Api.Weather.Controllers
{
    [ApiController]
    [Route("forecast/random")]
    public class RandomForecastController : ControllerBase
    {
        private readonly IApiHelper apiHelper;
        private readonly ILogger<RandomForecastController> logger;

        public RandomForecastController(IApiHelper apiHelper, ILogger<RandomForecastController> logger)
        {
            this.apiHelper = apiHelper;
            this.logger = logger;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            var min = Convert.ToDouble(forecast.Min(t => t));
            var max = Convert.ToDouble(forecast.Max(t => t));
            var tags = new Dictionary<string, string>();

            tags.Add("DeploymentType", "Environment");

            this.apiHelper.WavefrontSender.SendMetric("MinimumRandomForecast", min, DateTime.Now.Ticks, "Tap.Dotnet.Core.Api.Weather", tags);
            this.apiHelper.WavefrontSender.SendMetric("MaximumRandomForecast", max, DateTime.Now.Ticks, "Tap.Dotnet.Core.Api.Weather", tags);

            return forecast;
        }
    }
}
