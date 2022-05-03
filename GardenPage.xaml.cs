namespace iot_garden;

public partial class GardenPage : ContentPage
{
	int count = 0;

	public GardenPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		CounterLabel.Text = $"Current count: {count}";

		SemanticScreenReader.Announce(CounterLabel.Text);
	}
}

