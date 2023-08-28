using Newtonsoft.Json;
using System.Collections;
using System.Net;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Application.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

namespace Tap.Dotnet.Core.Web.Application
{
    public class WeatherApplication : IWeatherApplication
    {
        private readonly WavefrontDirectIngestionClient wfClient;

        public WeatherApplication(WavefrontDirectIngestionClient wfClient)
        {
            this.wfClient = wfClient;
        }

        public IList<WeatherForecastViewModel> GetWeatherForecastsByEnvironment(string zipCode)
        {
            var forecasts = new List<WeatherForecastViewModel>();
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
                        forecasts = JsonConvert.DeserializeObject<List<WeatherForecastViewModel>>(content);
                    }
                }
            }

            return forecasts;
        }

        public IList<WeatherForecastViewModel> GetWeatherForecastsByClaim(string zipCode)
        {
            var forecasts = new List<WeatherForecastViewModel>();

            var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT");

            var secretPath = Path.Combine(serviceBindings, "weather-api", "host");
            var weatherApi = System.IO.File.ReadAllText(secretPath);

            weatherApi = weatherApi.Trim();

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
                            
                        forecasts = JsonConvert.DeserializeObject<List<WeatherForecastViewModel>>(content);
                    }
                }
            }

            return forecasts;
        }

        public void GetByZipCode(string zipCode)
        {
            
            throw new NotImplementedException();
        }
    }
}