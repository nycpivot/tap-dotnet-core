using Tap.Dotnet.Core.Common;
using Tap.Dotnet.Core.Common.Interfaces;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

// get wavefront credentials
var wavefrontUrl = "https://vmwareprod.wavefront.com"; // Environment.GetEnvironmentVariable("WAVEFRONT_URL");
var wavefrontToken = "bb074869-ed55-4a94-8607-b384cf8a39c1"; // Environment.GetEnvironmentVariable("WAVEFRONT_TOKEN");

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
