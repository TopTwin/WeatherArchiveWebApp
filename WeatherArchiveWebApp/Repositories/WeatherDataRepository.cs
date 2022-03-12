using WeatherArchiveWebApp.Models;

namespace WeatherArchiveWebApp.Repositories
{
	public class WeatherDataRepository: IWeatherDataRepository
	{
		private readonly Db_Context context;

		public WeatherDataRepository(Db_Context context)
		{
			this.context = context;
		}

		public List<WeatherData> GetWeatherDataByYear(int year)
		{
			return context.weathersData
					.Select(w => w)
					.Where(w => w.Date.Year == year)
					.ToList();
		}

		public async Task<List<WeatherData>> SetEntitiesAsync(List<WeatherData> weatherDatas)
		{
			try
			{
				await context.AddRangeAsync(weatherDatas);
				await context.SaveChangesAsync();
			}
			catch(Exception ex)
			{
				return new List<WeatherData>();
			}

			return weatherDatas;
		}

		public List<WeatherData> GetWeatherData()
		{
			var result = context.weathersData.Select(w => w).ToList();
			return result;
		}
	}
}
