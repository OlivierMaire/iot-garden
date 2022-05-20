
using iot_garden_server.Services;
using iot_garden_shared.Services;

namespace iot_garden_server.Workers;

public class TimedSensorSettingWorker : IHostedService, IDisposable
{
    private readonly ILogger<TimedSensorSettingWorker> _logger;
    private Timer _timer = null!;

    private readonly SettingService _setting;
    private readonly ShareService _share;
    public TimedSensorSettingWorker(ILogger<TimedSensorSettingWorker> logger, SettingService setting, ShareService share)
    {
        _logger = logger;
        _setting = setting;
        _share = share;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed SensorSetting Worker running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(5));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed SensorSetting Worker is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }


    private async void DoWork(object? state)
    {
        // var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(
            "Timed SensorSetting Worker is loading settings.");

        _share.Garden = await _setting.LoadSettings(true);

        
        _logger.LogInformation(
            "Timed SensorSetting Worker, new settings loaded.");

    }
}
