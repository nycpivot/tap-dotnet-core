using Microsoft.AspNetCore.Mvc;

namespace Tap.Dotnet.Core.Api.Weather.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{zipcode}")]
        public IEnumerable<WeatherForecast> Get(string zipcode)
        {
            var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT");

            var hostFile = Path.Combine(serviceBindings, "weather-api", "host");
            var passwordFile = Path.Combine(serviceBindings, "weather-api", "password");

            var host = System.IO.File.ReadAllText(hostFile);
            var password = System.IO.File.ReadAllText(passwordFile);

            var currentWeatherUrl = new Uri($"{host}/current?postal_code={zipcode}&key={password}");
            var forecastUrl = new Uri($"{host}/forecast/daily?postal_code={zipcode}&key={password}");

            return null;
        }
    }
}