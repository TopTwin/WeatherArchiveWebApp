
namespace WeatherArchiveWebApp.Models
{
	public class WeatherData
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public float Temperature { get; set; }
		public float RelativeAirHumidity { get; set; }
		public float DewPoint { get; set; }
		public int AtmosphericPressure { get; set; }
		public string WindDirection { get; set; }
		public int WindSpeed { get; set; }
		public int CloudCover { get; set; }
		public int LowerCloudLimit { get; set; }
		public int HorizontalVisibility { get; set; }
		public string WeatherPhenomena { get; set; }
	}
}
