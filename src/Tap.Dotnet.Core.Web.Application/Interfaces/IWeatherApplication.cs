using Tap.Dotnet.Core.Web.Application.Models;

namespace Tap.Dotnet.Core.Web.Application.Interfaces
{
    public interface IWeatherApplication
    {
        IList<WeatherForecastViewModel> GetWeatherForecastsByEnvironment(string zipCode);
        IList<WeatherForecastViewModel> GetWeatherForecastsByClaim(string zipCode);

        void GetByZipCode(string zipCode);
    }
}
