using Tap.Dotnet.Core.Api.Weather.Interfaces;
using Tap.Dotnet.Core.Api.Weather.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);


// get location of claims
var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;

// get external weather API (weather-bit)
var weatherBitUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-bit", "host"));
var weatherBitKey = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-bit", "key"));

// get wavefront credentials
var wavefrontUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "host"));
var wavefrontToken = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "token"));

// create wavefront direct ingestion client
var wfClient = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();

var apiHelper = new ApiHelper()
{
    WeatherBitUrl = weatherBitUrl,
    WeatherBitKey = weatherBitKey,
    WavefrontDirectIngestionClient = wfClient
};

// Add services to the container.
builder.Services.AddSingleton<IApiHelper>(apiHelper);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
