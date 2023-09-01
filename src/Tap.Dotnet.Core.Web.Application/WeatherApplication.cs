﻿using Newtonsoft.Json;
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

        public HomeViewModel GetDefaultCriteria()
        {
            return new HomeViewModel() { ZipCode = this.apiHelper.DefaultZipCode };
        }

        public IList<WeatherForecastViewModel> GetForecast(string zipCode)
        {
            var forecast = new List<WeatherForecastViewModel>();

            try
            {
                var traceId = Guid.NewGuid();
                var spanId = Guid.NewGuid();

                this.apiHelper.WavefrontSender.SendSpan(
                    "Get", 0, 1, "ClaimController", traceId, spanId,
                    ImmutableList.Create(new Guid("82dd7b10-3d65-4a03-9226-24ff106b5041")), null,
                    ImmutableList.Create(
                        new KeyValuePair<string, string>("application", "tap-dotnet-core-web-mvc-claim"),
                        new KeyValuePair<string, string>("service", "ClaimController"),
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

                        var response = httpClient.GetAsync("forecast").Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var content = response.Content.ReadAsStringAsync().Result;
                            forecast = JsonConvert.DeserializeObject<List<WeatherForecastViewModel>>(content);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return forecast;
        }
    }
}