using Microsoft.EntityFrameworkCore;
using WeatherArchiveWebApp.Models;

namespace WeatherArchiveWebApp
{
	public class Db_Context : DbContext
	{
		public DbSet<WeatherData> weathersData { get; set; }

		public Db_Context(DbContextOptions<Db_Context> options)
			: base(options)
		{

		}
	}
}
