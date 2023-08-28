using Wavefront.SDK.CSharp.DirectIngestion;

namespace Tap.Dotnet.Core.Api.Weather.Interfaces
{
    public interface IApiHelper
    {
        string WeatherBitUrl { get; set; }
        string WeatherBitKey { get; set; }
        WavefrontDirectIngestionClient WavefrontDirectIngestionClient { get; set; }
    }
}
