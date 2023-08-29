using Tap.Dotnet.Core.Api.Weather.Interfaces;
using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Core.Api.Weather.Models
{
    public class ApiHelper : IApiHelper
    {
        private string weatherBitUrl = String.Empty;
        private string weatherBitKey = String.Empty;
        private IWavefrontSender wavefrontSender = null;

        public string WeatherBitUrl
        {
            get { return weatherBitUrl; }
            set { weatherBitUrl = value; }
        }

        public string WeatherBitKey
        {
            get { return weatherBitKey; }
            set { weatherBitKey = value; }
        }

        public IWavefrontSender WavefrontSender
        {
            get { return wavefrontSender; }
            set { wavefrontSender = value; }
        }
    }
}
