using Tap.Dotnet.Core.Web.Application.Interfaces;
using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Core.Web.Application.Models
{
    public class ApiHelper : IApiHelper
    {
        public string WeatherApi { get; set; } = String.Empty;
        public IWavefrontSender WavefrontSender { get; set; }
    }
}
