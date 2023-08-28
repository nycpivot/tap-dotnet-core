using Newtonsoft.Json;
using System.Net;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Application.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

namespace Tap.Dotnet.Core.Web.Application
{
    public class WeatherApplication : IWeatherApplication
    {
        private readonly string apiUrl;
        private readonly WavefrontDirectIngestionClient wfClient;

        public WeatherApplication(string apiUrl, WavefrontDirectIngestionClient wfClient)
        {
            this.apiUrl = apiUrl;
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
                    httpClient.BaseAddress = new Uri(apiUrl);

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
    }
}