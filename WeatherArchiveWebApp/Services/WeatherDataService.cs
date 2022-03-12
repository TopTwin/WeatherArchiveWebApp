using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WeatherArchiveWebApp.Models;
using WeatherArchiveWebApp.Repositories;

namespace WeatherArchiveWebApp.Services
{
	public class WeatherDataService: IWeatherDataService
	{
		private readonly IWeatherDataRepository weatherDataRepository;

		public WeatherDataService(IWeatherDataRepository weatherDataRepository)
		{
			this.weatherDataRepository = weatherDataRepository;
		}

		public List<WeatherData> GetWeatherData()
		{
			var listWeatherData = weatherDataRepository.GetWeatherData();
			return listWeatherData;
		}

		public List<WeatherData> GetSortedWeatherDataByMonthAndYear(int month, int year)
		{
			var sortWeatherData = weatherDataRepository.GetWeatherDataByYear(year);
			if (sortWeatherData.Count == 0)
				return sortWeatherData;
			else
				return sortWeatherData
						.Select(w => w)
						.Where(w => w.Date.Month == month)
						.ToList();
		}

		public List<WeatherData> GetWeatherDataByYear(int year)
		{
			return weatherDataRepository.GetWeatherDataByYear(year);
		}
		public async Task<List<WeatherData>> ParseXLSFiles(List<IFormFile> files)
		{
			var threads = new List<Thread>();
			var listWeatherData = new List<WeatherData>();
			XSSFWorkbook workBook;
			foreach(var file in files)
			{
				using (var fs = file.OpenReadStream())
				{
					workBook = new XSSFWorkbook(fs);
				}
				var thread = new Thread(() => ParseXLSFile(workBook, listWeatherData));
				thread.IsBackground = true;
				thread.Start();
				threads.Add(thread);
			}

			foreach (var thread in threads)
				thread.Join();

			var result = await weatherDataRepository.SetEntitiesAsync(listWeatherData);
			return result;
		}

		private List<WeatherData> ParseXLSFile(
			XSSFWorkbook workBook, 
			List<WeatherData> resultParseFile)
		{
			for (int i = 0; i < workBook.NumberOfSheets; i++)
			{
				ISheet sheet = workBook.GetSheetAt(i);

				resultParseFile.AddRange(ParseRows(sheet));
			}

			return resultParseFile;
		}

		private List<WeatherData> ParseRows(ISheet sheet)
		{
			var resultParseRows = new List<WeatherData>();
			for (int row = 4; row < sheet.LastRowNum; row++)
			{
				var rowSheet = sheet.GetRow(row);
				var weatherData = new WeatherData();
				if (rowSheet == null)
					continue;

				weatherData = ParseColumns(rowSheet);
				if (weatherData == null)
					continue;
				resultParseRows.Add(weatherData);
			}

			return resultParseRows;
		}
		
		private WeatherData ParseColumns(IRow rowSheet)
		{
			var weatherData = new WeatherData();
			for (int column = 0; column < rowSheet.LastCellNum; column++)
			{
				string value = " ";
				try
				{
					if (rowSheet.GetCell(column).CellType == CellType.Numeric)
						value = rowSheet.GetCell(column).NumericCellValue.ToString();
					else
						value = rowSheet.GetCell(column).StringCellValue;
				}
				catch (NullReferenceException ex)
				{
					return null;
				}

				switch (column)
				{
					case 0:
						{
							try
							{
								weatherData.Date = DateTime.ParseExact(value, "dd.MM.yyyy",
								   System.Globalization.CultureInfo.InvariantCulture);
							}
							catch (Exception ex)
							{
								return null;
							}
							break;
						}
					case 1:
						{
							try
							{
								DateTime dateTime = DateTime.ParseExact(value, "HH:mm",
								   System.Globalization.CultureInfo.InvariantCulture);
								weatherData.Date = weatherData.Date.AddHours(dateTime.Hour);
								weatherData.Date = weatherData.Date.AddMinutes(dateTime.Minute);
							}
							catch (Exception ex)
							{
								return null;
							}
							break;
						}
					case 2:
						{
							weatherData.Temperature =
									float.TryParse(value, out var result) ? result : 0;
							break;
						}
					case 3:
						{
							weatherData.RelativeAirHumidity =
									float.TryParse(value, out var result) ? result : 0;
							break;
						}
					case 4:
						{
							weatherData.DewPoint =
									float.TryParse(value, out var result) ? result : 0;
							break;
						}
					case 5:
						{
							weatherData.AtmosphericPressure =
									int.TryParse(value, out var result) ? result : 0; ;
							break;
						}
					case 6:
						{
							weatherData.WindDirection = value;
							break;
						}
					case 7:
						{
							weatherData.WindSpeed =
									int.TryParse(value, out var result) ? result : 0; ;
							break;
						}
					case 8:
						{
							weatherData.CloudCover =
									int.TryParse(value, out var result) ? result : 0;
							break;
						}
					case 9:
						{
							weatherData.LowerCloudLimit =
									int.TryParse(value, out var result) ? result : 0;
							break;
						}
					case 10:
						{
							weatherData.HorizontalVisibility =
									int.TryParse(value, out var result) ? result : 0;
							break;
						}
					case 11:
						{
							weatherData.WeatherPhenomena = value;
							break;
						}
				}
			}
			weatherData.WeatherPhenomena = weatherData.WeatherPhenomena ?? " ";
			weatherData.WindDirection = weatherData.WindDirection ?? " ";
			return weatherData;
		}
	}
}
