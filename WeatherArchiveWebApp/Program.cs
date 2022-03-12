using Microsoft.EntityFrameworkCore;
using WeatherArchiveWebApp;
using WeatherArchiveWebApp.Repositories;
using WeatherArchiveWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddRazorPages();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Db_Context>(options => options.UseSqlServer(connection));

builder.Services.AddTransient<IWeatherDataService, WeatherDataService>();
builder.Services.AddTransient<IWeatherDataRepository, WeatherDataRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=WeatherData}/{action=Index}/{id?}");

app.Run();
