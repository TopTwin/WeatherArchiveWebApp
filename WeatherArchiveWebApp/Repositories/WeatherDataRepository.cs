using Microsoft.EntityFrameworkCore;
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

		public async Task<List<WeatherData>> GetWeatherDataByYear(int year)
		{
			return await context.weathersData
					.Select(w => w)
					.Where(w => w.Date.Year == year)
					.ToListAsync();
		}

		public async Task<List<WeatherData>> GetWeatherDataByMonth(int month)
		{
			return await context.weathersData
					.Select(w => w)
					.Where(w => w.Date.Month == month)
					.ToListAsync();
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

		public async Task<List<WeatherData>> GetWeatherData()
		{
			return await context.weathersData.ToListAsync();
		}
	}
}
