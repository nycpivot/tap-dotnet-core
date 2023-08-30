using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Core.Common.Interfaces
{
    public interface IApiHelper
    {
        string WeatherApi { get; set; }
        IWavefrontSender WavefrontSender { get; set; }
    }
}
