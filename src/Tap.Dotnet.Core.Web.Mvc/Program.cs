using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Policy;
using Tap.Dotnet.Core.Web.Application;
using Tap.Dotnet.Core.Web.Application.Interfaces;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var wavefrontUrl = "";
var wavefrontToken = "";

builder.Services.AddSingleton(new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build());

builder.Services.AddScoped<IWeatherApplication, WeatherApplication>();

//builder.Services.AddHttpClient("Name").ConfigurePrimaryHttpMessageHandler(() => {
//var handler = new HttpClientHandler
//{
//    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
//    ServerCertificateCustomValidationCallback = (sender, certificate, chain, errors) =>
//    {
//        return true;
//    }
//}

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
