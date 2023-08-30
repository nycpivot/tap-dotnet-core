using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Tap.Dotnet.Core.Api.Weather.Interfaces;
using Wavefront.SDK.CSharp.DirectIngestion;

namespace Tap.Dotnet.Core.Api.Weather.Controllers
{
    [Route("forecast")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        private readonly IApiHelper apiHelper;

        private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly Counter TempsBelowZero = Metrics
            .CreateCounter("TempsBelowZero", "Number of temperatures below zero.",
                new CounterConfiguration { SuppressInitialValue = true });
        private static readonly Counter TempsAboveOneHundred = Metrics
            .CreateCounter("TempsAboveOneHundred", "Number of temperatures above one hundred.",
                new CounterConfiguration { SuppressInitialValue = true });

        public ForecastController(IApiHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        [HttpGet]
        [Route("random")]
        public IEnumerable<WeatherForecast> Get()
        {
            var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-100, 100),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            var tempsBelowZero = forecast.Count(t => t.TemperatureC < 0);
            var tempsAboveOneHundred = forecast.Count(t => t.TemperatureC > 100);

            if (tempsBelowZero > 0)
            {
                //TempsBelowZero;
            }

            return forecast;
        }

        [HttpGet]
        [Route("{zipCode}")]
        public IEnumerable<WeatherForecast> Get(string zipCode)
        {
            var url = this.apiHelper.WeatherBitUrl;
            var key = this.apiHelper.WeatherBitKey;

            var currentWeatherUrl = new Uri($"{url}/current?postal_code={zipCode}&key={key}");
            var forecastUrl = new Uri($"{url}/forecast/daily?postal_code={zipCode}&key={key}");

            return null;
        }
    }
}
