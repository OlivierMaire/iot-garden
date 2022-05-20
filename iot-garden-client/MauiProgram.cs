using CommunityToolkit.Maui;
using iot_garden.Services;
using iot_garden.ViewModels;
using iot_garden_shared.Services;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.ListView.Hosting;

namespace iot_garden;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			   .UseSkiaSharp(true)
			.UseMauiApp<App>()
			.ConfigureSyncfusionCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseMauiCommunityToolkit();
		builder.ConfigureSyncfusionListView();

		// dependency injection configuration
		//builder.Services.AddSingleton<INavigation, Microsoft.Maui.Controls.NaviNavigation>();

		builder.Services.AddSingleton<SettingService>();
		builder.Services.AddSingleton<IFirestoreService, FirestoreService>();
		// pages
		builder.Services.AddSingleton<SettingPage>();
		builder.Services.AddTransient<SensorEdit>();
		builder.Services.AddSingleton<GardenPage>();
		// viewmodels
		builder.Services.AddSingleton<SettingViewModel>();
		builder.Services.AddSingleton<SensorSettingViewModel>();
		builder.Services.AddSingleton<GardenViewModel>();
		


		var app = builder.Build();
		return app;
	}
}
