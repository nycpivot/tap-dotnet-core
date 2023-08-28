using Tap.Dotnet.Core.Web.Application;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Application.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;

var weatherApi = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-api", "host"));
var wavefrontUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "host"));
var wavefrontToken = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "password"));

var envVariable = new EnvironmentVariable() { Key = "WEATHER_API", Value = weatherApi };
var wfClient = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();

var weatherApplication = new WeatherApplication(envVariable, wfClient);

builder.Services.AddSingleton<IWeatherApplication>(weatherApplication);

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
