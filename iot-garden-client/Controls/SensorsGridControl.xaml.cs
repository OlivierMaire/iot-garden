using iot_garden_shared.Models;

namespace iot_garden.Controls;

public partial class SensorsGridControl : Grid
{

    public SensorsGridControl()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty SensorsProperty =
       BindableProperty.Create(nameof(Sensors), typeof(List<SensorSetting>), typeof(SensorsGridControl), null,
           BindingMode.OneWay, null, OnSensorsChanged);


    private static void OnSensorsChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var control = (SensorsGridControl)bindable;
        if (control != null)
        {
            control.Clear();
            if (newvalue is List<SensorSetting> sensors)
            {
                var rowNumber = -1;

                foreach (var sensorItem in sensors)
                {
                    var sensorCtl = new SensorControl(sensorItem);
                    //{
                    //    Sensor = sensorItem
                    //};

                    //grandTotal += totalItem.Value;


                    rowNumber++;
                    control.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        control.Add(sensorCtl, rowNumber % 2, (int)(rowNumber / 2));
                    //control.Add(valueLabel, 1, rowNumber);
                }

                //var grandTotalDescLabel = new Label { Text = "Total" };
                //var grandTotalValueLabel = new Label { Text = grandTotal.ToString("c") };

                //rowNumber++;
                //control.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                //control.Add(grandTotalDescLabel, 0, rowNumber);
                //control.Add(grandTotalValueLabel, 1, rowNumber);
            }
        }
    }

    public List<SensorSetting> Sensors
    {
        get => (List<SensorSetting>)GetValue(SensorsProperty);
        set => SetValue(SensorsProperty, value);
    }

}