using CommunityToolkit.Maui;
using iot_garden.Services;
using iot_garden.ViewModels;

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
		// dependency injection configuration
		builder.Services.AddSingleton<SettingService>();
		builder.Services.AddTransient<FirestoreService>();
		builder.Services.AddTransient<SettingPage>();
		builder.Services.AddTransient<SettingViewModel>();

		return builder.Build();
	}
}
