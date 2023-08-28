using Microsoft.AspNetCore.Mvc;
using Tap.Dotnet.Core.Api.Weather.Interfaces;
using Wavefront.SDK.CSharp.DirectIngestion;

namespace Tap.Dotnet.Core.Api.Weather.Controllers
{
    [Route("forecast")]
    [ApiController]
    public class RealForecastController : ControllerBase
    {
        private readonly IApiHelper apiHelper;

        public RealForecastController(IApiHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        [HttpGet]
        [Route("{zipCode}")]
        public IEnumerable<WeatherForecast> GetByZipCode(string zipCode)
        {
            var url = this.apiHelper.WeatherBitUrl;
            var key = this.apiHelper.WeatherBitKey;

            var currentWeatherUrl = new Uri($"{url}/current?postal_code={zipCode}&key={key}");
            var forecastUrl = new Uri($"{url}/forecast/daily?postal_code={zipCode}&key={key}");

            return null;
        }
    }
}
