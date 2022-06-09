using iot_garden_shared.Extensions;
using Microsoft.Maui.Dispatching;

namespace iot_garden.Controls;

public class SpriklerControl : GraphicsView
{
    private readonly SprinklerDrawable _drawable;

    private const double _fps = 30;

    private IDispatcherTimer Timer;
    public SpriklerControl()
    {
        _drawable = new SprinklerDrawable();
        Drawable = _drawable;

        var _fps = 30;
        var ms = 1000.0 / _fps;
        var ts = TimeSpan.FromMilliseconds(ms);
        //Device.StartTimer(ts, TimerLoop);
        Timer = this.Dispatcher.CreateTimer();
        Timer.Interval = ts;
        Timer.Tick += Timer_Tick;
        Timer.Start();
    }


    private void Timer_Tick(object sender, EventArgs e) {
        _drawable.Advance();
        this.Invalidate();
      //  Timer.Stop();

    }
    private bool TimerLoop()
    {
        // get the elapsed time from the stopwatch because the 1/30 timer interval is not accurate and can be off by 2 ms
        //var dt = _stopWatch.Elapsed.TotalSeconds;
        //_stopWatch.Restart();
        // calculate current fps
        //var fps = dt > 0 ? 1.0 / dt : 0;
        // when the fps is too low reduce the load by skipping the frame
        //if (fps < _fps / 2)
        //    return true;
        //_fpsCount++;
        //_fpsElapsed++;
        //if (_fpsCount == 20)
        //    _fpsCount = 0;
        ////Its been a second
        //if (_fpsElapsed == _fps)
        //{
        //    _fpsElapsed = 0;
        //    Drawable.AlienFire();
        //}
        //Invalidate();
        //return true;
        _drawable.Advance();

        //Device.BeginInvokeOnMainThread(() => Invalidate());
        this.Dispatcher.Dispatch(() => Invalidate());
        return true;
    }

}

public class SprinklerDrawable : IDrawable
{
    private readonly Drop[] Drops;

    public SprinklerState State;

    public SprinklerDrawable()
    {
        Drops = new Drop[4];


    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.Purple;
        canvas.FillRectangle(dirtyRect);
        canvas.FillColor = Colors.Blue;

        for (int i = 0; i < Drops.Length; i++)
        {
            if (Drops[i] == null)
            {
                Drops[i] = Randomize(dirtyRect.Width, dirtyRect.Height);
                if (i >= (Drops.Length /2))
                {
                    Drops[i].Angle = 360 - Drops[i].Angle;
                    Drops[i].X = dirtyRect.Width - Drops[i].X;
                    //Drops[i].Vel *= -1;
                    Drops[i].Invert = true;
                }
            }
            Drops[i]?.Draw(canvas, dirtyRect);
        }


    }

    public void Advance()
    {

        for (int i = 0; i < Drops.Length; i++)
        {
            Drops[i]?.Advance();
        }
    }


    public Drop Randomize(double width, double height)
    {
        Random rand = new();
        //for (int i = 0; i < Drops.Length; i++)
        //{
        //    Drops[i] = new()
        return new Drop()
        {
            X = (float)(rand.NextDouble() * width / 4 + width / 4),
          //  Y = (float)(rand.NextDouble() * height / 4 + height / 4 - 10),
          Y = (float)(rand.NextDouble() * height / 4 + height / 4),
            SizeRatio = (float)(rand.NextDouble() * 10 + 10),
            //XVel = rand.NextDouble() - .5,
            //YVel = rand.NextDouble() - .5,
            Vel = rand.NextDouble() * 10.0 + 6,

            Angle = (float)(rand.NextDouble() * 30) + 120f
                //R = (byte)rand.Next(50, 255),
                //G = (byte)rand.Next(50, 255),
                //B = (byte)rand.Next(50, 255),
            };
        //}
    }
}

public class Drop
{
    public float X;
    public float Y;
    public float? InitY;

    public float Angle;
    public float SizeRatio;

    //public double XVel;
    //public double YVel;

    public float LaunchAngleRadians;
    public double Vel;
    public float Time = 0;


    private PathF Path;

    public Drop()
    {
        Path = new PathF();
        // 20,25 to 0,25 
        Path.AddArc(0, 15, 20, 35, 0, 180, true);
        // 0,25  to 10,0
        Path.CurveTo(3.195f, 12.638f, 5.972f, 6.388f, 10, 0);
        // 10,0 to 20,25
        Path.CurveTo(14.028f, 6.388f, 16.805f, 12.638f, 20, 25);
        Path.Close();
    }

    public bool Invert { get; internal set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    { 

        canvas.SaveState();
        //canvas.Translate(X, Y);
        canvas.Translate(X, dirtyRect.Height - Y);
        //canvas.Rotate(Angle, X + 10, Y + 20);
        canvas.Rotate(Angle, 10, 20);
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 6;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.StrokeLineJoin = LineJoin.Round;
        canvas.FillPath(Path);
        canvas.DrawPath(Path);


        //canvas.Rotate(-1 * Angle, X+10, Y+20);
        // canvas.Rotate(-1 * Angle, X, Y);
        //canvas.Translate(-1 * X, -1 * Y);
        canvas.RestoreState();
        //canvas.Rotate(-1 * Angle, 10,20);

        //canvas.Translate(-1 * X, -1*Y);

        // draw curve
        if (!InitY.HasValue)
        {
            InitY = Y;
            LaunchAngleRadians = (Angle > 180 ? 270 - Angle : Angle - 90).ToRadians();
            //LaunchAngle = 45;
            //Vel = 16;
        }
        //var p = new PathF();
        //p.MoveTo(X- (Invert ? Time : Time * -1) + 10, dirtyRect.Height + 20 - GetPosition(0));
        //for (var x = 0; x < 200; x += 2)
        //{
        //    p.LineTo(X - (Invert ? Time : Time * -1) + 10 + (Invert ? x : x * -1), dirtyRect.Height + 20 - GetPosition(x));
        //}

        //canvas.StrokeColor = Colors.Aqua;
        //canvas.StrokeSize = 2;
        //canvas.DrawPath(p);



        //var x1 = Time - 50;
        //var y1 = GetSlope(Time, Y, Time - 50);
        //var x2 = Time + 50;
        //var y2 = GetSlope(Time, Y, Time + 50);

        //var x3 = x1;// > x2 ? x1 : x2;
        //var y3 = x1 > x2 ? y1 : y2;


        //var p2 = new PathF();
        ////p2.MoveTo(X - (Invert ? Time : Time * -1) + 10, dirtyRect.Height + 20 - GetSlope(Time, Y, 0));
        //p2.MoveTo(X - (Invert ? Time : Time * -1) + 10 + (Invert ? x1 : x1 * -1), dirtyRect.Height + 20 - GetSlope(Time, Y, x1));
        //p2.LineTo(X - (Invert ? Time : Time * -1) + 10 + (Invert ? x2 : x2 * -1), dirtyRect.Height + 20 - GetSlope(Time, Y, x2));
        //p2.LineTo(X - (Invert ? Time : Time * -1) + 10 + (Invert ? x3 : x3 * -1), dirtyRect.Height + 20 - y3 );
        //p2.Close();
        //canvas.StrokeColor = Colors.Green;
        //canvas.StrokeSize = 1;
        //canvas.DrawPath(p2);


    }

    private float GetPosition(float t)
    {
        float g = 9.8067f;

        float y = 0;
        y = (float)(InitY + (t * Math.Tan(LaunchAngleRadians)) - (g * Math.Pow(t / Vel * Math.Cos(LaunchAngleRadians), 2) / 2));
        return y;
    }

    private float GetSlope(float x, float y, float t)
    {
        float g = 9.8067f;
        double m = Math.Tan(LaunchAngleRadians) - ((Math.Pow(Math.Cos(LaunchAngleRadians), 2) * g * x) / Math.Pow(Vel, 2));

        var y1 = m * (t - x) + y;
        return (float)y1;
    }

    public void Advance()
    {
        // reset loop
        if (Time > 200)
        {
            X -= Invert ? Time : Time *-1 ;
            Time -= Time;
        }

        Time += 1;
        X += Invert ? 1 : -1;
        Y = GetPosition(Time);



        var x1 = Time - 50;
        var y1 = GetSlope(Time, Y, Time - 50);
        var x2 = Time + 50;
        var y2 = GetSlope(Time, Y, Time + 50);

        var x3 = x1;// > x2 ? x1 : x2;
        var y3 = x1 > x2 ? y1 : y2;

        var a = Math.Abs(y1 - y2);
        var b = 100;
        var c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        var ang_a = ((float)Math.Asin(a / c)).ToDegrees();
        var ang_b = ((float)Math.Asin(b / c)).ToDegrees();
        Angle = y1 >= y2 ? Invert ? (ang_a-90) : ang_b : Invert ? (ang_b +180): (ang_a+90);
        //Angle = Invert ? Angle + 90 : Angle;
    }


}

public enum SprinklerState
{
    Stopped, 
    Starting,
    Loop,
    Stopping
}