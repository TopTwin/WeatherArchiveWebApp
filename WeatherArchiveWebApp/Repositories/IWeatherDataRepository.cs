using WeatherArchiveWebApp.Models;

namespace WeatherArchiveWebApp.Repositories
{
	public interface IWeatherDataRepository
	{
		public Task<List<WeatherData>> SetEntitiesAsync(List<WeatherData> weatherDatas);
		public Task<List<WeatherData>> GetWeatherData();
		public Task<List<WeatherData>> GetWeatherDataByYear(int year);
		public Task<List<WeatherData>> GetWeatherDataByMonth(int month);
	}
}
