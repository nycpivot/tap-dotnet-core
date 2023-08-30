using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace Tap.Dotnet.Core.Api.Weather.Controllers
{
    [ApiController]
    [Route("forecast/random")]
    public class RandomForecastController : ControllerBase
    {
        private static readonly Counter TempsBelowZero = Metrics
            .CreateCounter("TempsBelowZero", "Number of temperatures below zero.",
                new CounterConfiguration { SuppressInitialValue = true });
        private static readonly Counter TempsAboveOneHundred = Metrics
            .CreateCounter("TempsAboveOneHundred", "Number of temperatures above one hundred.",
                new CounterConfiguration { SuppressInitialValue = true });

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
            var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-100, 100),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            var tempsBelowZero = forecast.Count(t => t.TemperatureC < 0);
            var tempsAboveOneHundred = forecast.Count(t => t.TemperatureC > 100);

            if(tempsBelowZero > 0)
            {
                //TempsBelowZero;
            }

            return forecast;
        }
    }
}
