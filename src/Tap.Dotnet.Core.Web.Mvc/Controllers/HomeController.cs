using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Mvc.Models;

namespace Tap.Dotnet.Core.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherApplication weatherApplication;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}