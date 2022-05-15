using Google.Api.Gax.Grpc.GrpcNetClient;
using Google.Cloud.Firestore;
using iot_garden.Services;
using iot_garden.ViewModels;

namespace iot_garden;

public partial class SensorEdit : ContentPage
{
	private readonly SettingService _setting;

	public SensorEdit(SettingService setting, SensorSettingViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		_setting = setting;

	}

    protected override bool OnBackButtonPressed()
    {
        return false;
        //return base.OnBackButtonPressed();
    }

    private async void Close(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }
    private async void SaveData(object sender, EventArgs e)
    {
        ((SensorSettingViewModel)BindingContext).SensorData.Id = Guid.NewGuid().ToString();
        _setting.Settings.Sensors.Add(
        ((SensorSettingViewModel)BindingContext).SensorData
        );
        await _setting.SaveSettings(_setting.Settings);
        await Navigation.PopModalAsync();

    }

}

