using System.Reflection;
using CommunityToolkit.Maui;
using ipr2.Application;
using ipr2.Persistence;
using ipr2.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ipr2;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		string settingsStream = "ipr2.appsettings.json";

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var a = Assembly.GetExecutingAssembly();
		using var stream = a.GetManifestResourceStream(settingsStream)!;
		builder.Configuration.AddJsonStream(stream);

		var connStr = builder.Configuration.GetConnectionString("SqliteConnection")!;
		string dataDirectory = FileSystem.Current.AppDataDirectory + "/";
		connStr = string.Format(connStr, dataDirectory);

		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(connStr)
			.Options;

		builder.Services
			.AddApplication()
			.AddPersistence(options)
			.RegisterPages()
			.RegisterViewModels();

		// Task.Run avoids a deadlock on the UI thread's synchronization context
		Task.Run(() => DbInitializer.Initialize(builder.Services.BuildServiceProvider())).Wait();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	static IServiceCollection RegisterPages(this IServiceCollection services)
	{
		services
			.AddSingleton<BrandsPage>()
			.AddTransient<AdDetailsPage>()
			.AddTransient<AdEditPage>()
			.AddTransient<BrandEditPage>();
		return services;
	}

	static IServiceCollection RegisterViewModels(this IServiceCollection services)
	{
		services
			.AddSingleton<BrandsViewModel>()
			.AddTransient<AdDetailsViewModel>()
			.AddTransient<AdEditViewModel>()
			.AddTransient<BrandEditViewModel>();
		return services;
	}
}
