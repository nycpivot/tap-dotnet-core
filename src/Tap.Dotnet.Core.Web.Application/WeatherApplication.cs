using Newtonsoft.Json;
using System.Net;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Application.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

namespace Tap.Dotnet.Core.Web.Application
{
    public class WeatherApplication : IWeatherApplication
    {
        private readonly EnvironmentVariable environmentVariable;
        private readonly WavefrontDirectIngestionClient wfClient;

        public WeatherApplication(EnvironmentVariable environmentVariable, WavefrontDirectIngestionClient wfClient)
        {
            this.environmentVariable = environmentVariable;
            this.wfClient = wfClient;
        }

        public IList<WeatherForecastViewModel> GetRandomForecastsByEnvironment()
        {
            var forecasts = new List<WeatherForecastViewModel>();

            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.environmentVariable.Value);

                    //var response = await httpClient.GetAsync("weatherforecast");
                    //response.EnsureSuccessStatusCode();

                    var response = httpClient.GetAsync("forecast/random").Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        forecasts = JsonConvert.DeserializeObject<List<WeatherForecastViewModel>>(content);
                    }
                }
            }

            return forecasts;
        }

        public IList<WeatherForecastViewModel> GetRandomForecastsByClaim()
        {
            var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT");
            var weatherApi = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-api", "host"));

            var secretPath = Path.Combine(serviceBindings, "weather-api", "host");
            ViewBag.SecretPath = secretPath;

            var weatherApi = System.IO.File.ReadAllText(secretPath);
            ViewBag.WeatherApi = weatherApi;

            weatherApi = weatherApi.Trim();
        }
    }
}