using Newtonsoft.Json;
using System.Collections.Immutable;
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

        public IList<WeatherForecastViewModel> GetRandomForecastsByEnvironment(Guid traceId)
        {
            var forecasts = new List<WeatherForecastViewModel>();

            this.apiHelper.WavefrontSender.SendSpan(
                "GetRandomForecastsByEnvironment", 0, 0, "WeatherApplication", traceId, Guid.NewGuid(),
                ImmutableList.Create(new Guid("82dd7b10-3d65-4a03-9226-24ff106b5041")), null,
                ImmutableList.Create(
                    new KeyValuePair<string, string>("application", "tap-dotnet-core-web-mvc-claim"),
                    new KeyValuePair<string, string>("service", "WeatherApplication"),
                    new KeyValuePair<string, string>("http.method", "GET")), null);

            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.apiHelper.WeatherApiUrl);

                    httpClient.DefaultRequestHeaders.Add("X-TraceId", traceId.ToString());

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
    }
}