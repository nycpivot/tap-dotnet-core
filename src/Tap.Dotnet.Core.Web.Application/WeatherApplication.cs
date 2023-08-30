using Newtonsoft.Json;
using System.Net;
using Tap.Dotnet.Core.Common.Interfaces;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Application.Models;

namespace Tap.Dotnet.Core.Web.Application
{
    public class WeatherApplication : IWeatherApplication
    {
        private readonly IApiHelper apiHelper;

        public WeatherApplication(IApiHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        public IList<WeatherForecastViewModel> GetWeatherForecastByZipCode(string zipCode)
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
                    httpClient.BaseAddress = new Uri(this.apiHelper.WeatherApi);

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
