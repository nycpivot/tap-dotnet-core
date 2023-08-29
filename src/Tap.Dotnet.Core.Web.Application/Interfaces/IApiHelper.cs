using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tap.Dotnet.Core.Web.Application.Interfaces
{
    public interface IApiHelper
    {
        string WeatherApiUrl { get; set; }
        IWavefrontSender WavefrontSender { get; set; }
    }
}
