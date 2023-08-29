﻿using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Immutable;
using Tap.Dotnet.Core.Common.Interfaces;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Mvc.Models;
using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Core.Web.Mvc.Controllers
{
    public class EnvironmentController : Controller
    {
        private readonly IWeatherApplication weatherApplication;
        private readonly IApiHelper apiHelper;
        private readonly ILogger<HomeController> _logger;

        public EnvironmentController(
            IWeatherApplication weatherApplication,
            IApiHelper apiHelper,
            ILogger<HomeController> logger)
        {
            this.weatherApplication = weatherApplication;
            this.apiHelper = apiHelper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var traceId = Guid.NewGuid();
            var spanId = Guid.NewGuid();

            var start = DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow);

            // list environment variables
            try
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

                ViewBag.Variables = variables.OrderBy(e => e.Key).ToList();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Environment", ex.StackTrace ?? ex.Message);
            }

            try
            {
                var end = DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow);

                this.apiHelper.WavefrontSender.SendSpan(
                    "Get", start, end, "EnvironmentController", traceId, spanId,
                    ImmutableList.Create(new Guid("2f64e538-9457-11e8-9eb6-529269fb1459")), null,
                    ImmutableList.Create(
                        new KeyValuePair<string, string>("application", "tap-dotnet-core-web-mvc-env"),
                        new KeyValuePair<string, string>("service", "EnvironmentController"),
                        new KeyValuePair<string, string>("http.method", "GET")), null);

                var forecasts = this.weatherApplication.GetRandomForecastsByEnvironment(traceId);

                if (forecasts != null && forecasts.Count == 5)
                {
                    ViewBag.Forecast1 = forecasts[0];
                    ViewBag.Forecast2 = forecasts[1];
                    ViewBag.Forecast3 = forecasts[2];
                    ViewBag.Forecast4 = forecasts[3];
                    ViewBag.Forecast5 = forecasts[4];
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Index", ex.StackTrace ?? ex.Message);
            }

            return View();
        }
    }
}
