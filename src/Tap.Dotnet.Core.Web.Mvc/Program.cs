using Tap.Dotnet.Core.Common;
using Tap.Dotnet.Core.Common.Interfaces;
using Tap.Dotnet.Core.Web.Application;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

var weatherApi = Environment.GetEnvironmentVariable("WEATHER_API") ?? String.Empty;
var wavefrontUrl = Environment.GetEnvironmentVariable("WAVEFRONT_URL") ?? String.Empty;
var wavefrontToken = Environment.GetEnvironmentVariable("WAVEFRONT_TOKEN") ?? String.Empty;

var wfSender = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();

var apiHelper = new ApiHelper()
{
    WeatherApiUrl = weatherApi,
    WavefrontSender = wfSender
};

builder.Services.AddSingleton<IApiHelper>(apiHelper);
builder.Services.AddScoped<IWeatherApplication, WeatherApplication>();

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
