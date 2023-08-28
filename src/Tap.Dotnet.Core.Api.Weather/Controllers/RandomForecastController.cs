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

        public RandomForecastController(IApiHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<RandomForecastController> _logger;

        public RandomForecastController(ILogger<RandomForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
