using iot_garden.Models;
using iot_garden.ViewModels;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;

namespace iot_garden.Controls;

public partial class SensorControl : ContentView
{

    public SensorSetting Sensor { get; set; }
    private ObservableCollection<SensorData> _data;
    //private SfCartesianChart chart;
    private CartesianChart chart;
    public ISeries[] SensorSeries { get; set; } = new ISeries[]
            {
                new StepLineSeries<SensorData>
                {
                    TooltipLabelFormatter =
                    chartPoint => $"{new DateTime((long) chartPoint.SecondaryValue):HH:mm:ss}: {chartPoint.PrimaryValue}",
                    Values =new ObservableCollection<SensorData> { },
        Mapping = (data, point) =>
        {
            // this function will be called for every city in our data collection
            // in this case Tokio, New York and Mexico city
            // it takes the city and the point in the chart liveCharts built for the given city
            // you must map the coordinates to the point

            // use the Population property as the primary value (normally Y)
            point.PrimaryValue = data.Value;

            // use the index of the city in our data collection as the secondary value
            // (normally X)
            point.SecondaryValue = data.Timestamp.Ticks;
        }
                }

            };

    public Axis[] XAxes { get; set; } = new Axis[]
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("HH:mm:ss"),
                LabelsRotation = 15,

                // when using a date time type, let the library know your unit 
                UnitWidth = TimeSpan.FromMinutes(1).Ticks, 

                // if the difference between our points is in hours then we would:
                // UnitWidth = TimeSpan.FromHours(1).Ticks,

                // since all the months and years have a different number of days
                // we can use the average, it would not cause any visible error in the user interface
                // Months: TimeSpan.FromDays(30.4375).Ticks
                // Years: TimeSpan.FromDays(365.25).Ticks

                // The MinStep property forces the separator to be greater than 1 day.
                MinStep = TimeSpan.FromMinutes(1).Ticks
}
        };



    public SensorControl(SensorSetting Sensor)
    {
        this.Sensor = Sensor;
        _data = new ObservableCollection<SensorData>();
        InitializeComponent();

        var stackLayout = new VerticalStackLayout();

        var descLabel = new Label { Text = Sensor.Name };
        var valueLabel = new Label { Text = Sensor.Type.ToString() };
        var lastDataLabel = new Label { BindingContext = this };
        lastDataLabel.SetBinding(Label.TextProperty, nameof(LastDataValue));
        stackLayout.Add(
            new FlexLayout() { Children = { descLabel, lastDataLabel}, Direction=FlexDirection.Row, Wrap= FlexWrap.NoWrap, JustifyContent=FlexJustify.SpaceBetween, HeightRequest = 30 });

            
        stackLayout.Add(valueLabel);


        chart = new CartesianChart()
        {
            BindingContext = this,
            XAxes = XAxes,
            HeightRequest = 100,
        };
        chart.SetBinding(CartesianChart.SeriesProperty, nameof(SensorSeries));


        //    // chart
        //     chart = new SfCartesianChart();

        //    chart.Title = new Label
        //    {
        //        Text = "Value"
        //    };
        //    // Initializing primary axis
        //    DateTimeAxis primaryAxis = new DateTimeAxis()
        //    {
        //        //RangePadding = DateTimeRangePadding.Additional,

        //    };
        //    primaryAxis.Title = new ChartAxisTitle()
        //    {
        //        Text = "Time"
        //};
        //    chart.XAxes.Add(primaryAxis);

        //    //Initializing secondary Axis
        //    NumericalAxis secondaryAxis = new NumericalAxis()
        //    {
        //        RangePadding = NumericalPadding.Additional

        //    };
        //    secondaryAxis.Title = new ChartAxisTitle()
        //    {
        //        Text = "Value"
        //    };
        //    chart.YAxes.Add(secondaryAxis);
        //    //chart.BindingContext = this;
        //    dataBinding = new SensorDataViewModel();
        //    chart.BindingContext = dataBinding;
        //    //Initialize the two series for SfChart
        //    var binding = new Binding() { Path = "Data" };
        //    LineSeries series = new LineSeries()
        //    {
        //        Label = "Height",
        //        ShowDataLabels = true,

        //        XBindingPath = "Timestamp",
        //        YBindingPath = "Value",
        //        DataLabelSettings = new CartesianDataLabelSettings
        //        {
        //            LabelPlacement = DataLabelPlacement.Inner
        //        }
        //    };
        //    series.SetBinding(LineSeries.ItemsSourceProperty, binding);
        //    series.EnableAnimation = true;
        //    //chart.AnimateSeries();
        //    //series.SetBinding(LineSeries.ItemsSourceProperty, nameof(_data));

        //    chart.Series.Add(series);

        //stackLayout.Add(chart);
        AbsoluteLayout.SetLayoutBounds(stackLayout, new Rect(0,0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.SizeProportional);

        AbsoluteLayout.SetLayoutBounds(chart, new Rect(0, 15, 1, 1));
        AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.SizeProportional); 

        Content = new AbsoluteLayout
        {
            Children = { chart , stackLayout }
            , HeightRequest = 100
        };

   


        MessagingCenter.Subscribe<GardenViewModel, SensorData>(this, Sensor.Id, async (sender, data) =>
        {
            _data.Add(data);

            //dataBinding.Data.Add(data);
            OnPropertyChanged(nameof(LastDataValue));
            //OnPropertyChanged(nameof(Data));
            ((ObservableCollection<SensorData>)SensorSeries[0].Values.Cast<SensorData>()).Add(data);
            //OnPropertyChanged(nameof(Series));
            //chart.Series[0].ItemsSource = _data;
        });

    }

    public string LastDataValue
    {
        get => _data.LastOrDefault()?.Value.ToString();
    }

    public ObservableCollection<SensorData> Data
    {
        get => _data;
    }


}