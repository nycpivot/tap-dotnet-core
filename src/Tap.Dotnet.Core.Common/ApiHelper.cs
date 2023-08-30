using Tap.Dotnet.Core.Common.Interfaces;
using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Core.Common
{
    public class ApiHelper : IApiHelper
    {
        public string WeatherApi { get; set; } = String.Empty;

        public IWavefrontSender WavefrontSender { get; set; }
    }
}
