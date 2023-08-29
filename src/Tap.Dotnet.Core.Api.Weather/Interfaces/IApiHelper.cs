using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Core.Api.Weather.Interfaces
{
    public interface IApiHelper
    {
        string WeatherBitUrl { get; set; }
        string WeatherBitKey { get; set; }
        IWavefrontSender WavefrontSender { get; set; }
    }
}
