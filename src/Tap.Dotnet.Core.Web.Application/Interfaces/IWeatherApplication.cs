using Tap.Dotnet.Core.Web.Application.Models;

namespace Tap.Dotnet.Core.Web.Application.Interfaces
{
    public interface IWeatherApplication
    {
        IList<EnvironmentVariable> GetEnvironmentVariables();

        IList<WeatherForecastViewModel> GetWeatherForecastsByEnvironment(string zipCode);

        void GetByZipCode(string zipCode);
    }
}
