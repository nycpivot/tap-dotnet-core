using App.Metrics;
using App.Metrics.Reporting.Wavefront.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using Tap.Dotnet.Core.Api.Weather.Models;
using Tap.Dotnet.Core.Common.Interfaces;
using Wavefront.SDK.CSharp.Common;
using Wavefront.SDK.CSharp.Common.Application;

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
            var start = DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow);

            var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            var min = Convert.ToDouble(forecast.Min(t => t.TemperatureC));
            var max = Convert.ToDouble(forecast.Max(t => t.TemperatureC));
            var tags = new Dictionary<string, string>();

            tags.Add("DeploymentType", "Environment");

            // save as storage in wavefront
            this.apiHelper.WavefrontSender.SendMetric("MinimumRandomForecast", min, 
                DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow), "tap-dotnet-core-api-weather-env", tags);
            this.apiHelper.WavefrontSender.SendMetric("MaximumRandomForecast", max,
                DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow), "tap-dotnet-core-api-weather-env", tags);

            // report metrics
            var applicationTags = new ApplicationTags.Builder("tap-dotnet-core-api-weather-env", "random-forecast-controller").Build();

            var metricsBuilder = new MetricsBuilder();

            metricsBuilder.Report.ToWavefront(
              options =>
              {
                  options.WavefrontSender = this.apiHelper.WavefrontSender;
                  options.Source = "tap-dotnet-core-api-weather-env"; // optional
                  options.WavefrontHistogram.ReportMinuteDistribution = true; // optional
                  options.ApplicationTags = applicationTags;
              });

            var end = DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow);

            var traceHeader = this.Request.Headers["X-TraceId"];

            this.apiHelper.WavefrontSender.SendSpan(
                "GetRandomWeatherForecast", start, end, "RandomForecastController",
                new Guid(traceHeader[0]), Guid.NewGuid(),
                ImmutableList.Create(new Guid("2f64e538-9457-11e8-9eb6-529269fb1459")), null,
                ImmutableList.Create(
                    new KeyValuePair<string, string>("application", "tap-dotnet-core-api-weather-env"),
                    new KeyValuePair<string, string>("service", "random-forecast-controller"),
                    new KeyValuePair<string, string>("http.method", "GET")), null);

            return forecast;
        }
    }
}
