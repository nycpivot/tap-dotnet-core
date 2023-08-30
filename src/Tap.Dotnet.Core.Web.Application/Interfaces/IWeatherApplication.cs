using Tap.Dotnet.Core.Web.Application.Models;

namespace Tap.Dotnet.Core.Web.Application.Interfaces
{
    public interface IWeatherApplication
    {
        IList<WeatherForecastViewModel> GetWeatherForecastByZipCode(string zipCode);
    }
}
