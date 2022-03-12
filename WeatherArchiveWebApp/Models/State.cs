namespace WeatherArchiveWebApp.Models
{
	public static class State
	{
		public enum Method
		{
			ShowAllWeatherData = 0,
			SortWeatherData = 1
		}

		static public Method ControllerMethod { get; set; }
		static public int Month { get; set; }
		static public int Year { get; set; }
		static public int NumberPage { get; set; } = 1;
	}
}
