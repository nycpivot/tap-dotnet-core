using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Tap.Dotnet.Core.Web.Mvc.Models;

namespace Tap.Dotnet.Core.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri("https://tap-dotnet-core.default.run-eks.tap.nycpivot.com");

                    //var response = await httpClient.GetAsync("weatherforecast");
                    //response.EnsureSuccessStatusCode();

                    var response = httpClient.GetAsync("weatherforecast").Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        var forecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);

                        if (forecasts != null && forecasts.Count == 5)
                        {
                            ViewBag.Forecast1 = forecasts[0];
                            ViewBag.Forecast2 = forecasts[1];
                            ViewBag.Forecast3 = forecasts[2];
                            ViewBag.Forecast4 = forecasts[3];
                            ViewBag.Forecast5 = forecasts[4];
                        }
                    }
                }
            }

            return View();
        }

        public IActionResult Privacy()
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