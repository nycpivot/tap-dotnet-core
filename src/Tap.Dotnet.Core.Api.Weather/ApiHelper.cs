using Tap.Dotnet.Core.Api.Weather.Interfaces;

namespace Tap.Dotnet.Core.Api.Weather
{
    public class ApiHelper : IApiHelper
    {
        public string WeatherBitUrl { get; set; } = String.Empty;
        public string WeatherBitKey { get; set; } = String.Empty;
    }
}
