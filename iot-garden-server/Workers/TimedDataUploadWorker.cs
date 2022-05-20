
using iot_garden_server.Services;
using iot_garden_shared.Models;
using iot_garden_shared.Services;

namespace iot_garden_server.Workers;

public class TimedDataUploadWorker : IHostedService, IDisposable
{
    private readonly ILogger<TimedDataUploadWorker> _logger;
    private Timer _timer = null!;

    private readonly SettingService _setting;
    private readonly DataService _data;
    private readonly ShareService _share;

    public TimedDataUploadWorker(ILogger<TimedDataUploadWorker> logger, ShareService share, DataService data)
    {
        _logger = logger;
        _share = share;
        _data = data;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed DataUpload Worker running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed DataUpload Worker is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }


    private async void DoWork(object? state)
    {
        // var count = Interlocked.Increment(ref executionCount);
        if (_share.Garden == null || _share.Garden.Sensors == null || _share.Garden.Sensors.Count <= 0)
        {
            _logger.LogInformation(
                    "Timed DataUpload Worker: no garden found, skip work.");
            return;
        }

        _logger.LogInformation(
            "Timed DataUpload Worker is getting data.");

        foreach (var sensor in _share.Garden.Sensors)
        {
            // get data from sensor

            SensorData data = new SensorData()
            {
                SensorId = sensor.Id,
                Timestamp = DateTime.UtcNow,
                Value = new Random().Next(50)
            };

            if (sensor.Type == SensorType.Moisture)
                _share.LastHumidity = data.Value;

            await _data.UploadData(data);
        }

        _logger.LogInformation(
            "Timed DataUpload Worker, data uploaded.");

    }
}
