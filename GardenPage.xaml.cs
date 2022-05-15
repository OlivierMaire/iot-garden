using iot_garden.ViewModels;

namespace iot_garden;

public partial class GardenPage : ContentPage
{

	public GardenPage(GardenViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
	 
}

