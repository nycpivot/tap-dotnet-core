using Tap.Dotnet.Core.Common;
using Tap.Dotnet.Core.Common.Interfaces;
using Tap.Dotnet.Core.Web.Application;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Application.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;

var weatherApi = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-api", "host"));
var wavefrontUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-url", "host"));
var wavefrontToken = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-token", "token"));

var wfSender = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();

var apiHelper = new ApiHelper()
{
    WeatherApiUrl = weatherApi,
    WavefrontSender = wfSender
};

builder.Services.AddSingleton<IApiHelper>(apiHelper);
builder.Services.AddSingleton<IWeatherApplication, WeatherApplication>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
