using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Mvc.Models;

namespace Tap.Dotnet.Core.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherApplication weatherApplication;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IWeatherApplication weatherApplication,
            ILogger<HomeController> logger)
        {
            this.weatherApplication = weatherApplication;

            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //private static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
        //{
        //    // It is possible to inspect the certificate provided by the server.
        //    Console.WriteLine($"Requested URI: {requestMessage.RequestUri}");
        //    Console.WriteLine($"Effective date: {certificate?.GetEffectiveDateString()}");
        //    Console.WriteLine($"Exp date: {certificate?.GetExpirationDateString()}");
        //    Console.WriteLine($"Issuer: {certificate?.Issuer}");
        //    Console.WriteLine($"Subject: {certificate?.Subject}");

        //    // Based on the custom logic it is possible to decide whether the client considers certificate valid or not
        //    Console.WriteLine($"Errors: {sslErrors}");
        //    return sslErrors == SslPolicyErrors.None;
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}