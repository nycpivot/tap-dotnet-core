using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
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
            try
            {
                var weatherApi = Environment.GetEnvironmentVariable("WEATHER_API")
                    ?? "https://tap-dotnet-core-api-weather.default.run-eks.tap.nycpivot.com";

                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                    {
                        return true;
                    };

                    using (var httpClient = new HttpClient(handler))
                    {
                        httpClient.BaseAddress = new Uri(weatherApi);

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
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Index", ex.StackTrace ?? ex.Message);
            }

            return View();
        }

        public IActionResult Export()
        {
            try
            {
                var variables = new List<EnvironmentVariable>();

                var environment = Environment.GetEnvironmentVariables();
                foreach(DictionaryEntry variable in environment)
                {
                    var ev = new EnvironmentVariable() 
                    { 
                        Key = variable.Key.ToString() ?? String.Empty, 
                        Value = variable.Value?.ToString() ?? String.Empty
                    };

                    variables.Add(ev);
                }

                ViewBag.Variables = variables.OrderBy(e => e.Key).ToList();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Export", ex.StackTrace ?? ex.Message);
            }

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