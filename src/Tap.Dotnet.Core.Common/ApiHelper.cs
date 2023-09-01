using StackExchange.Redis;
using Tap.Dotnet.Core.Common.Interfaces;
using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Core.Common
{
    public class ApiHelper : IApiHelper
    {
        private string weatherApiUrl = String.Empty;
        private IWavefrontSender wavefrontSender = null;

        public string WeatherApiUrl { get; set; } = String.Empty;

        public IWavefrontSender WavefrontSender { get; set;} = null;

        public IDatabase CacheDb { get; set; } = null;
    }
}
