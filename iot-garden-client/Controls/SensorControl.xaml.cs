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
using iot_garden_shared.Extensions;
using System.Drawing;

namespace iot_garden.Controls;

public partial class SensorControl : ContentView, IDrawable
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

        var stackLayout = new VerticalStackLayout()
        {
            Padding = new Thickness(5, 0)
        };

        var descLabel = new Label { Text = Sensor.Name };
        //var valueLabel = new Label { Text = Sensor.Type.ToString() };
        var lastDataLabel = new Label { BindingContext = this, FontSize = 20, FontAttributes = FontAttributes.Bold };
        lastDataLabel.SetBinding(Label.TextProperty, nameof(LastDataValue));
        stackLayout.Add(
            new FlexLayout() { Children = { descLabel, lastDataLabel}, Direction=FlexDirection.Row, Wrap= FlexWrap.NoWrap, JustifyContent=FlexJustify.SpaceBetween, HeightRequest = 30 });


        //stackLayout.Add(valueLabel);


        chart = new CartesianChart()
        {
            BindingContext = this,

            XAxes = new Axis[]
        {
            new Axis
            {
                Labeler = value => null, //new DateTime((long) value).ToString("HH:mm"),
                                         //    LabelsRotation = -30,
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
                Labeler = value => value == 0 ? "" : value.ToString(),

                

}
        },
        HeightRequest = 100,
        Padding = new Thickness(0, 0, 0, 15),
        };
        chart.SetBinding(CartesianChart.SeriesProperty, nameof(SensorSeries));

        AbsoluteLayout.SetLayoutBounds(stackLayout, new Rect(0,0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.SizeProportional);

        AbsoluteLayout.SetLayoutBounds(chart, new Rect(0, 15, 1, 1));
        AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.SizeProportional);

        var icon = new Image()
        {
            Source = ImageSource.FromFile(Sensor.Type.AsIconName()),
            WidthRequest = 32,
            HeightRequest = 32,
            Style = Resources["SensorIcon"] as Style
        };
        

        AbsoluteLayout.SetLayoutBounds(icon, new Rect(10, 0.35, 32, 32));
        AbsoluteLayout.SetLayoutFlags(icon, AbsoluteLayoutFlags.YProportional);


        GraphicsView graphics = new GraphicsView()
        {
            //HeightRequest = 100
        };
        graphics.Drawable = this;


        AbsoluteLayout.SetLayoutBounds(graphics, new Rect(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(graphics, AbsoluteLayoutFlags.SizeProportional);



        Content = new AbsoluteLayout
        {
            Children = { chart, graphics, icon , stackLayout }
            , HeightRequest = 100
        };

        //graphics.Invalidate();
        


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
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        Microsoft.Maui.Graphics.RadialGradientPaint radialGradientPaint = new Microsoft.Maui.Graphics.RadialGradientPaint
        {
            //StartColor = Colors.Red,
            //EndColor = Colors.DarkBlue,

            StartColor = Colors.Yellow,
            EndColor = Colors.Green,
            Center = new Microsoft.Maui.Graphics.Point(1.0, 1.0)
           
            // Radius is already 0.5
        };

        RectF radialRectangle = dirtyRect;
        canvas.Alpha = 0.35F;
        canvas.StrokeSize = 2;
        canvas.SetFillPaint(radialGradientPaint, radialRectangle);
        canvas.SetShadow(new Microsoft.Maui.Graphics.SizeF(10, 10), 10, Colors.Grey);
        canvas.FillRoundedRectangle(radialRectangle, 6);

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