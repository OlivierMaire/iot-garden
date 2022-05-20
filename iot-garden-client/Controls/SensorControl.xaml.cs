using iot_garden_shared.Models;
using iot_garden.ViewModels;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace iot_garden.Controls;

public partial class SensorControl : ContentView
{

    public SensorSetting Sensor { get; set; }
    private ObservableCollection<SensorData> _data;
    //private SfCartesianChart chart;
    private CartesianChart chart;
    //private SensorDataViewModel dataBinding;
    public ISeries[] SensorSeries { get; set; }

    public Axis[] XAxes { get; set; } = new Axis[]
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("HH:mm"),
                LabelsRotation = -30,
                Padding = new LiveChartsCore.Drawing.Padding(0),
                

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


        SensorSeries = new ISeries[]
            {
                new ColumnSeries<SensorData>
                {
            DataPadding = new LiveChartsCore.Drawing.LvcPoint(0,0),
                  MaxBarWidth = double.MaxValue,
                        GroupPadding = 0,

                    //GeometrySize = 0,
                    //Stroke = new SolidColorPaint { Color = SKColors.Blue, StrokeThickness = 2 },
                    TooltipLabelFormatter =
                    chartPoint => $"{new DateTime((long) chartPoint.SecondaryValue):HH:mm:ss}: {chartPoint.PrimaryValue}",
                    Values = _data,
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

            XAxes = new Axis[]
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("HH:mm"),
                LabelsRotation = -30,
                Padding = new LiveChartsCore.Drawing.Padding(5),

                UnitWidth = TimeSpan.FromMinutes(1).Ticks, 
                MinStep = TimeSpan.FromMinutes(1).Ticks
}
        },
            YAxes = new Axis[]
        {
            new Axis
            {
                Padding = new LiveChartsCore.Drawing.Padding(5),
                MinLimit = 0,

}
        },
        HeightRequest = 100,
        };
        chart.SetBinding(CartesianChart.SeriesProperty, nameof(SensorSeries));


        //// chart
        //var chart2 = new SfCartesianChart() { 
        //     HeightRequest = 100
        //};

        ////chart.Title = new Label
        ////{
        ////    Text = "Value"
        ////};
        //// Initializing primary axis
        //DateTimeAxis primaryAxis = new DateTimeAxis()
        //{
        //    //RangePadding = DateTimeRangePadding.Additional,

        //};
        //////primaryAxis.Title = new ChartAxisTitle()
        //////{
        //////    Text = "Time"
        //////};
        //chart2.XAxes.Add(primaryAxis);

        //////Initializing secondary Axis
        //NumericalAxis secondaryAxis = new NumericalAxis()
        //{
        //    RangePadding = NumericalPadding.RoundEnd
        //};
        //////secondaryAxis.Title = new ChartAxisTitle()
        //////{
        //////    Text = "Value"
        //////};
        //chart2.YAxes.Add(secondaryAxis);

        ////chart2.XAxes.Add(new NumericalAxis());
        ////chart2.YAxes.Add(new NumericalAxis());

        ////chart.BindingContext = this;
        //dataBinding = new SensorDataViewModel();
        //chart2.BindingContext = this;
        ////Initialize the two series for SfChart
        //var binding = new Binding() { Path = "Data" };
        //ColumnSeries series = new ColumnSeries()
        //{
        //    //Label = "Height",
        //    //ShowDataLabels = true,

        //    Width = 1,
        //    //XBindingPath = "X",
        //    //YBindingPath = "Y"
        //    XBindingPath = "Timestamp",
        //    YBindingPath = "Value",
        //    //DataLabelSettings = new CartesianDataLabelSettings
        //    //{
        //    //    LabelPlacement = DataLabelPlacement.Inner
        //    //}
        //};
        ////series.EnableAnimation = true;
        //series.SetBinding(ColumnSeries.ItemsSourceProperty, binding);
        ////chart.AnimateSeries();
        ////series.SetBinding(LineSeries.ItemsSourceProperty, nameof(_data));

        //chart2.Series.Add(series);

        //stackLayout.Add(chart2);


        //Button button = new Button();
        //button.Text = "Dynamic";
        //button.Clicked += Button_Clicked;


        AbsoluteLayout.SetLayoutBounds(stackLayout, new Rect(0,0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.SizeProportional);

        AbsoluteLayout.SetLayoutBounds(chart, new Rect(0, 15, 1, 1));
        AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.SizeProportional);


        //AbsoluteLayout.SetLayoutBounds(chart2, new Rect(0, 15, 1, 1));
        //AbsoluteLayout.SetLayoutFlags(chart2, AbsoluteLayoutFlags.SizeProportional);

        Content = new AbsoluteLayout
        {
            Children = { chart , stackLayout }
            , HeightRequest = 100
        };




        MessagingCenter.Subscribe<GardenViewModel, SensorData>(this, Sensor.Id, async (sender, data) =>
        {
            this.Dispatcher.Dispatch(() =>
            {

                //dataBinding.Data.Add(data);
                //var count = dataBinding.Data.Count(d => d.SensorId == data.SensorId);
                //if (count > 10)
                //{
                //    var dataToRemove = dataBinding.Data.Where(d => d.SensorId == data.SensorId).Take(count - 10);
                //    foreach (var dr in dataToRemove)
                //        dataBinding.Data.Remove(dr);
                //}
                while (_data.Count > 10)
                    _data.RemoveAt(0);
                _data.Add(data);
                OnPropertyChanged(nameof(_data));
                OnPropertyChanged(nameof(LastDataValue));
                //((ObservableCollection<SensorData>)SensorSeries[0].Values.Cast<SensorData>()).Add(data);

                //Random rnd = new Random();
                //var data2 = new Model() { X = dataBinding.Data.Count, Y = rnd.Next(20, 200) };
                //dataBinding.Data.Add(data2);

            });
            //dataBinding.Data.Add(data);
            //OnPropertyChanged(nameof(Data));
            //OnPropertyChanged(nameof(Series));
            //chart.Series[0].ItemsSource = _data;
            //dataBinding.Data.Add(data);
            //((SensorDataViewModel)chart2.BindingContext).Data.Add(data);
           
            //Device.BeginInvokeOnMainThread(() => {


            //    Random rnd = new Random();
            //    var data2 = new Model() { X = dataBinding.Data.Count, Y = rnd.Next(20, 200) };
            //    dataBinding.Data.Add(data2);

            //});

        });

    }


    //private void Button_Clicked(object sender, EventArgs e)
    //{
    //    Random rnd = new Random();
    //    var data = new Model() { X = dataBinding.Data.Count, Y = rnd.Next(20, 200) };
    //    dataBinding.Data.Add(data);
    //}

    public string LastDataValue
    {
        get => _data.LastOrDefault()?.Value.ToString();
    }

    public ObservableCollection<SensorData> Data
    {
        get => _data;
    }


}