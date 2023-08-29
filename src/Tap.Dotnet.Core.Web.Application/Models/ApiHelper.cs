using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tap.Dotnet.Core.Web.Application.Models
{
    public class ApiHelper : IApiHelper
    {
        public string WeatherApiUrl { get; set; }
        public IWavefrontSender WavefrontSender { get; set; }
    }
}
