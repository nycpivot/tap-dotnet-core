using Tap.Dotnet.Core.Api.Weather.Interfaces;
using Wavefront.SDK.CSharp.DirectIngestion;

namespace Tap.Dotnet.Core.Api.Weather.Models
{
    public class ApiHelper : IApiHelper
    {
        private string weatherBitUrl = String.Empty;
        private string weatherBitKey = String.Empty;
        private WavefrontDirectIngestionClient wavefrontDirectIngestionClient = null;

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

        public WavefrontDirectIngestionClient WavefrontDirectIngestionClient
        {
            get { return wavefrontDirectIngestionClient; }
            set { wavefrontDirectIngestionClient= value; }
        }
    }
}
