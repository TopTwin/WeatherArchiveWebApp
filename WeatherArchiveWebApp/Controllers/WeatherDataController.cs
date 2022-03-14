using Microsoft.AspNetCore.Mvc;
using WeatherArchiveWebApp.Models;
using WeatherArchiveWebApp.Services;
using X.PagedList;

namespace WeatherArchiveWebApp.Controllers
{
	public class WeatherDataController : Controller
	{
		private readonly IWeatherDataService weatherDataService;

		public WeatherDataController(IWeatherDataService weatherDataService)
		{
			this.weatherDataService = weatherDataService;
		}
		
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Upload()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UploadFile(List<IFormFile> files)
		{
			var badFiles = new List<IFormFile>();
			foreach(var file in files)
			{
				var fileName = file.FileName;
				var format = fileName.Substring(fileName.Length - 5, 5);
				if(format != ".xlsx")
				{
					badFiles.Add(file);
				}
			}
			foreach(var badFile in badFiles)
				files.Remove(badFile);

			if(files.Count > 0)
			{
				await weatherDataService.ParseXLSFiles(files);
			}
			return RedirectToAction("Index");
		}

		public IActionResult ListWeatherData()
		{
			return View(new List<WeatherData>().ToPagedList(1,50));
		}

		[HttpGet]
		public async Task<IActionResult> ShowWeatherData(int month, int year, int? page)
		{
			List<WeatherData> listWeatherData;
			if (month != 13 || year != 1)
				if (month == 13)
					listWeatherData = await weatherDataService
							.GetWeatherDataByYear(year);
				else if (year == 1)
					listWeatherData = await weatherDataService
							.GetWeatherDataByMonth(month);
				else
					listWeatherData = await weatherDataService
							.GetSortedWeatherDataByMonthAndYear(month, year);
			else
				listWeatherData = await weatherDataService.GetWeatherData();

			int pageNumber = page ?? 1;
			ViewBag.Month = month;
			ViewBag.Year = year;
			return View("ListWeatherData", listWeatherData.ToPagedList(pageNumber, 50));
		}

		public IActionResult BackToHome()
		{
			return View("Index");
		}
	}
}
