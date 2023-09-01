using Tap.Dotnet.Core.Web.Application.Models;

namespace Tap.Dotnet.Core.Web.Application.Interfaces
{
    public interface IWeatherApplication
    {
        HomeViewModel GetDefaultCriteria();
        IList<WeatherForecastViewModel> GetForecast(string zipCode);
    }
}
