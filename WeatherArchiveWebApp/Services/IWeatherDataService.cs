using WeatherArchiveWebApp.Models;

namespace WeatherArchiveWebApp.Services
{
	public interface IWeatherDataService
	{
		public Task<List<WeatherData>> ParseXLSFiles(List<IFormFile> files);
		public Task<List<WeatherData>> GetWeatherData();
		public Task<List<WeatherData>> GetSortedWeatherDataByMonthAndYear(int month, int year);
		public Task<List<WeatherData>> GetWeatherDataByYear(int year);
		public Task<List<WeatherData>> GetWeatherDataByMonth(int month);
	}
}
