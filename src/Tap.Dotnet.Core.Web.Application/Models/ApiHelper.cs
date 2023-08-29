using Tap.Dotnet.Core.Web.Application.Interfaces;
using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Core.Web.Application.Models
{
    public class ApiHelper : IApiHelper
    {
        private string weatherApiUrl = String.Empty;
        private IWavefrontSender wavefrontSender = null;

        public string WeatherApiUrl
        {
            get { return weatherApiUrl; }
            set { weatherApiUrl = value; }
        }

        public IWavefrontSender WavefrontSender
        {
            get { return wavefrontSender; }
            set { wavefrontSender = value; }
        }
    }
}
