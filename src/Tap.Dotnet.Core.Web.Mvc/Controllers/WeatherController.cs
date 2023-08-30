using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Tap.Dotnet.Core.Common.Interfaces;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Application.Models;
using Tap.Dotnet.Core.Web.Mvc.Models;

namespace Tap.Dotnet.Core.Web.Mvc.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherApplication weatherApplication;
        private readonly IApiHelper apiHelper;
        private readonly ILogger<HomeController> _logger;

        public WeatherController(
            IWeatherApplication weatherApplication,
            IApiHelper apiHelper,
            ILogger<HomeController> logger)
        {
            this.weatherApplication = weatherApplication;
            this.apiHelper = apiHelper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var forecasts = this.weatherApplication.GetWeatherForecastByZipCode("10001");

                if (forecasts != null && forecasts.Count == 5)
                {
                    ViewBag.Forecast1 = forecasts[0];
                    ViewBag.Forecast2 = forecasts[1];
                    ViewBag.Forecast3 = forecasts[2];
                    ViewBag.Forecast4 = forecasts[3];
                    ViewBag.Forecast5 = forecasts[4];
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Index", ex.StackTrace ?? ex.Message);
            }

            // list environment variables
            try
            {
                var variables = new List<EnvironmentVariable>();

                var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;

                var secretPath = Path.Combine(serviceBindings, "weather-api", "host");
                ViewBag.SecretPath = secretPath;

                var weatherApi = System.IO.File.ReadAllText(secretPath);
                ViewBag.WeatherApi = weatherApi;

                try
                {
                    var environment = Environment.GetEnvironmentVariables();
                    foreach (DictionaryEntry variable in environment)
                    {
                        var ev = new EnvironmentVariable()
                        {
                            Key = variable.Key.ToString() ?? String.Empty,
                            Value = variable.Value?.ToString() ?? String.Empty
                        };

                        variables.Add(ev);
                    }
                }
                catch
                {
                    // send errors somewhere
                }

                ViewBag.Variables = variables.OrderBy(e => e.Key).ToList();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Environment", ex.StackTrace ?? ex.Message);
            }

            return View();
        }
    }
}
