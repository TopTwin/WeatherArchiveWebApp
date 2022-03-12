using WeatherArchiveWebApp.Models;

namespace WeatherArchiveWebApp.Services
{
	public interface IWeatherDataService
	{
		public Task<List<WeatherData>> ParseXLSFiles(List<IFormFile> files);
		public List<WeatherData> GetWeatherData();
		public List<WeatherData> GetSortedWeatherDataByMonthAndYear(int month, int year);
		public List<WeatherData> GetWeatherDataByYear(int year);
	}
}
