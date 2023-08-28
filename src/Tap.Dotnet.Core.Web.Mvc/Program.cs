using Tap.Dotnet.Core.Web.Application;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Tap.Dotnet.Core.Web.Application.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var weatherApi = Environment.GetEnvironmentVariable("WEATHER_API");
var wavefrontUrl = Environment.GetEnvironmentVariable("WAVEFRONT_URL");
var wavefrontToken = Environment.GetEnvironmentVariable("WAVEFRONT_TOKEN");

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
