using iot_garden.ViewModels;
using iot_garden_shared.Services;

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

	private async void AddSensor(object sender, EventArgs e)
	{
        await Navigation.PushModalAsync(new SensorEdit(_setting, new SensorSettingViewModel()));
    } 
}

