using Microsoft.AspNetCore.Mvc;
using Wavefront.SDK.CSharp.DirectIngestion;

namespace Tap.Dotnet.Core.Api.Weather.Controllers
{
    [Route("forecast")]
    [ApiController]
    public class RealForecastController : ControllerBase
    {
        private readonly WavefrontDirectIngestionClient wfClient;

        public RealForecastController()
        {
            var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;

            var wavefrontUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "host"));
            var wavefrontToken = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "password"));

            this.wfClient = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();
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
