namespace Tap.Dotnet.Core.Domain
{
    public class WeatherBitDaily
    {
        public string city_name { get; set; }
        public string state_code { get; set; }
        public string country_code { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string timezone { get; set; }

        public WeatherBitDescription weather { get; set; } = new WeatherBitDescription();
    }
}
