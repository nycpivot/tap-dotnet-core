using Tap.Dotnet.Core.Api.Weather.Interfaces;
using Tap.Dotnet.Core.Api.Weather.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

// get wavefront credentials
var wavefrontUrl = Environment.GetEnvironmentVariable("WAVEFRONT_URL");
var wavefrontToken = Environment.GetEnvironmentVariable("WAVEFRONT_TOKEN");

// create wavefront direct ingestion client
var wfSender = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();

var apiHelper = new ApiHelper()
{
    WavefrontSender = wfSender
};

// Add services to the container.
builder.Services.AddSingleton<IApiHelper>(apiHelper);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
