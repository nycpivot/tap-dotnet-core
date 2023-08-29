using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Core.Common.Interfaces
{
    public interface IApiHelper
    {
        string WeatherApiUrl { get; set; }
        IWavefrontSender WavefrontSender { get; set; }
    }
}
