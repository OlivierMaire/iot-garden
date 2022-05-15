using DependencyInjectionMauiBlazor;
using iot_garden.Models;
using iot_garden.ViewModels;
using MessagePipe;

namespace iot_garden.Controls;

public partial class SensorControl : ContentView
{

    readonly ISubscriber<string, SensorData> _subscriber;
    readonly IDisposable _disposable;
    public SensorSetting Sensor { get; set; }
    private List<SensorData> _data;
    public SensorControl(SensorSetting Sensor/*ISubscriber<string, SensorData> subscriber*/)
    {
        this.Sensor = Sensor;
        InitializeComponent();

        var descLabel = new Label { Text = Sensor.Name };
        var valueLabel = new Label { Text = Sensor.Type.ToString() };
        var lastDataLabel = new Label { Text = "lastData" };
        _data = new List<SensorData>();
        lastDataLabel.BindingContext = this;
        lastDataLabel.SetBinding(Label.TextProperty, nameof(LastDataValue));

        this.Stack.Add(descLabel);
        this.Stack.Add(valueLabel);
        this.Stack.Add(lastDataLabel);

        MessagingCenter.Subscribe<GardenViewModel, SensorData>(this, Sensor.Id, async (sender, data) =>
        {
            _data.Add(data); 
            OnPropertyChanged(nameof(LastDataValue));
        });
        //_subscriber = Resolver.ServiceProvider.GetRequiredService<ISubscriber<string, SensorData>>();
        ////_subscriber = subscriber;
        //var bag = DisposableBag.CreateBuilder(); // composite disposable for manage subscription
        //_subscriber.Subscribe(Sensor.Id, x => { _data.Add(x); OnPropertyChanged(nameof(LastDataValue)); }).AddTo(bag);
        //_disposable = bag.Build();

    }

    public string LastDataValue
    {
        get => _data.LastOrDefault()?.Value.ToString();
        //set
        //{
        //    _lastUpdated = value;
        //    OnPropertyChanged(nameof(LastUpdated));
        //}
    }


    //public static readonly BindableProperty SensorProperty =
    //   BindableProperty.Create(nameof(Sensor), typeof(SensorSetting), typeof(SensorControl), null,
    //       BindingMode.OneWay, null, OnSensorChanged);


    //private static void OnSensorChanged(BindableObject bindable, object oldvalue, object newvalue)
    //{
    //    var control = (SensorControl)bindable;
    //    if (control != null)
    //    {
    //        if (newvalue is SensorSetting sensor)
    //        { 

    //                var descLabel = new Label { Text = sensor.Name };
    //                var valueLabel = new Label { Text = sensor.Type.ToString() };

    //                control.Stack.Add(descLabel);
    //                control.Stack.Add(valueLabel);

    //        }
    //    }
    //}

    //   public SensorSetting Sensor
    //{
    //	get => (SensorSetting)GetValue(SensorProperty);
    //	set => SetValue(SensorProperty, value);
    //   }
}