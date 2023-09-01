using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tap.Dotnet.Core.Common.Interfaces;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Mvc.Models;

namespace Tap.Dotnet.Core.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherApplication weatherApplication;
        private readonly IApiHelper apiHelper;
        private readonly ILogger<HomeController> logger;

        public HomeController(
            IWeatherApplication weatherApplication,
            IApiHelper apiHelper, ILogger<HomeController> logger)
        {
            this.weatherApplication = weatherApplication;
            this.apiHelper = apiHelper;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var zipCode = this.weatherApplication.GetDefaultZipCode();
                var forecasts = this.weatherApplication.GetForecast(zipCode);

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

            return View();
        }

        [HttpPost]
        public ActionResult Search(HomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO: SubscribeUser(model.Email);
            }

            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
