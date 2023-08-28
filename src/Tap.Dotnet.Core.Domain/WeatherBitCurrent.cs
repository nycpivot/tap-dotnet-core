namespace Tap.Dotnet.Core.Domain
{
    public class WeatherBitCurrent
    {
        public string city_name { get; set; }
        public string state_code { get; set; }
        public string country_code { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string timezone { get; set; }
        public string temp { get; set; }
        public string clouds { get; set; }
        public string datetime { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string precip { get; set; }
        public string snow { get; set; }

        public WeatherBitDescription weather { get; set; } = new WeatherBitDescription();
    }
}
