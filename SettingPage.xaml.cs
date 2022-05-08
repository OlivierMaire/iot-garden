using Google.Api.Gax.Grpc.GrpcNetClient;
using Google.Cloud.Firestore;
using iot_garden.Services;
using iot_garden.ViewModels;

namespace iot_garden;

public partial class SettingPage : ContentPage
{
	private readonly SettingService _setting;

	public SettingPage(SettingService setting, SettingViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();

		_setting = setting;
	}

	private async void LoadData(object sender, EventArgs e)
	{
		await _setting.LoadSettings(true);
	}
	private async void SaveData(object sender, EventArgs e)
	{
		await _setting.SaveSettings();
	}

}

