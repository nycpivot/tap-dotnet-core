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

        public IList<EnvironmentVariable> GetEnvironmentVariables()
        {
            var variables = new List<EnvironmentVariable>();

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

            return variables;
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

        public void GetByZipCode(string zipCode)
        {
            
            throw new NotImplementedException();
        }
    }
}