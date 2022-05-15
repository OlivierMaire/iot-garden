using CommunityToolkit.Maui;
using DependencyInjectionMauiBlazor;
using iot_garden.Services;
using iot_garden.ViewModels;
using MessagePipe;
using Syncfusion.Maui.ListView.Hosting;

namespace iot_garden;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseMauiCommunityToolkit();
		builder.ConfigureSyncfusionListView();

		builder.Services.AddMessagePipe();

		// dependency injection configuration
		//builder.Services.AddSingleton<INavigation, Microsoft.Maui.Controls.NaviNavigation>();

		builder.Services.AddSingleton<SettingService>();
		builder.Services.AddSingleton<FirestoreService>();
		// pages
		builder.Services.AddSingleton<SettingPage>();
		builder.Services.AddTransient<SensorEdit>();
		builder.Services.AddSingleton<GardenPage>();
		// viewmodels
		builder.Services.AddSingleton<SettingViewModel>();
		builder.Services.AddSingleton<SensorSettingViewModel>();
		builder.Services.AddSingleton<GardenViewModel>();
		


		var app = builder.Build();
		app.Services.UseResolver(); 
		return app;
	}
}
