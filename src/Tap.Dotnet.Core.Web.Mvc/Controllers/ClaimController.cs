using Microsoft.AspNetCore.Mvc;
using Tap.Dotnet.Core.Web.Application.Interfaces;

namespace Tap.Dotnet.Core.Web.Mvc.Controllers
{
    public class ClaimController : Controller
    {
        private readonly IWeatherApplication weatherApplication;
        private readonly ILogger<HomeController> _logger;

        public ClaimController(
            IWeatherApplication weatherApplication,
            ILogger<HomeController> logger)
        {
            this.weatherApplication = weatherApplication;

            _logger = logger;
        }

        public IActionResult Index()
        {
            // list environment variables
            try
            {
                var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;

                var secretPath = Path.Combine(serviceBindings, "weather-api", "host");
                ViewBag.SecretPath = secretPath;

                var weatherApi = System.IO.File.ReadAllText(secretPath);
                ViewBag.WeatherApi = weatherApi;

                weatherApi = weatherApi.Trim();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Environment", ex.StackTrace ?? ex.Message);
            }

            //try
            //{
            //    var forecasts = this.weatherApplication.GetWeatherForecastsByEnvironment("");

            //    if (forecasts != null && forecasts.Count == 5)
            //    {
            //        ViewBag.Forecast1 = forecasts[0];
            //        ViewBag.Forecast2 = forecasts[1];
            //        ViewBag.Forecast3 = forecasts[2];
            //        ViewBag.Forecast4 = forecasts[3];
            //        ViewBag.Forecast5 = forecasts[4];
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("Index", ex.StackTrace ?? ex.Message);
            //}

            return View();

        }
    }
}
