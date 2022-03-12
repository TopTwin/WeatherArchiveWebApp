using WeatherArchiveWebApp.Models;

namespace WeatherArchiveWebApp.Repositories
{
	public interface IWeatherDataRepository
	{
		public Task<List<WeatherData>> SetEntitiesAsync(List<WeatherData> weatherDatas);
		public List<WeatherData> GetWeatherData();
		public List<WeatherData> GetWeatherDataByYear(int year);
	}
}
