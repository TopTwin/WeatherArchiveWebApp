using Microsoft.AspNetCore.Mvc;
using WeatherArchiveWebApp.Models;
using WeatherArchiveWebApp.Services;

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
				var result = await weatherDataService.ParseXLSFiles(files);
			}
			return RedirectToAction("Index");
		}

		public IActionResult ListWeatherData()
		{
			return View(new List<WeatherData>());
		}

		[HttpGet]
		public IActionResult SortWeatherData(int month, int year)
		{
			if(State.Year != year || 
				State.Month != month || 
				State.ControllerMethod != State.Method.SortWeatherData)
				State.NumberPage = 1;

			State.Year = year;
			State.Month = month;
			State.ControllerMethod = State.Method.SortWeatherData;

			var sortWeatherData = new List<WeatherData>();
			if (month == 13)
				sortWeatherData = weatherDataService
						.GetWeatherDataByYear(State.Year);
			else
				sortWeatherData = weatherDataService
						.GetSortedWeatherDataByMonthAndYear(State.Month, State.Year);

			ViewBag.Number = State.NumberPage;
			ViewBag.MaxPage = sortWeatherData.Count / 50 + 1;
			ViewBag.Month = State.Month;
			ViewBag.Year = State.Year;
			return View("ListWeatherData", sortWeatherData);
		}

		[HttpGet]
		public IActionResult ShowAllWeatherData()
		{
			if(State.ControllerMethod != State.Method.ShowAllWeatherData)
				State.NumberPage = 1;
			State.ControllerMethod = State.Method.ShowAllWeatherData;

			var allWeatherData = weatherDataService.GetWeatherData();

			ViewBag.Number = State.NumberPage;
			ViewBag.MaxPage = allWeatherData.Count / 50 + 1;
			return View("ListWeatherData", allWeatherData);
		}

		public IActionResult BackToHome()
		{
			return View("Index");
		}

		public IActionResult NextPage(int number)
		{
			State.NumberPage = number + 1;
			if (State.ControllerMethod == State.Method.SortWeatherData)
				return RedirectToAction("SortWeatherData",
					new
					{
						month = State.Month,
						year = State.Year
					});
			else
				return RedirectToAction("ShowAllWeatherData");
		}

		public IActionResult PreviousPage(int number)
		{
			State.NumberPage = number - 1;
			if (State.ControllerMethod == State.Method.SortWeatherData)
				return RedirectToAction("SortWeatherData",
					new
					{
						month = State.Month,
						year = State.Year
					});
			else
				return RedirectToAction("ShowAllWeatherData");
		}
	}
}
