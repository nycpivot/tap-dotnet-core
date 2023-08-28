using Microsoft.AspNetCore.Http.Extensions;
using Tap.Dotnet.Core.Api.Weather.Interfaces;
using Tap.Dotnet.Core.Api.Weather.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);


// get wavefront credentials
var wavefrontUrl = Environment.GetEnvironmentVariable("WAVEFRONT-API");
var wavefrontToken = Environment.GetEnvironmentVariable("WAVEFRONT-TOKEN");

// create wavefront direct ingestion client
var wfClient = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();

var apiHelper = new ApiHelper()
{
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
