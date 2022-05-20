using iot_garden_server.Services;

namespace iot_garden_server;

public class ValveWorker : BackgroundService
{
    private readonly ILogger<ValveWorker> _logger;
    private readonly ShareService _share;

    private Dictionary<string, bool> _valveState;

    public ValveWorker(ILogger<ValveWorker> logger, ShareService share)
    {
        _logger = logger;
        _share = share;
        _valveState = new Dictionary<string, bool>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            
            // no setting
                if (_share.Garden == null || _share.Garden.Sensors == null || _share.Garden.Sensors.Count <= 0)
                {
                    _logger.LogInformation(
                            "Valve Worker: no garden found, skip work.");
                    return;
                }
            // infinite loop
            if (_valveState.Count() <= 0)
            {
                

                var relays = _share.Garden.Sensors.Where(s => s.Type == iot_garden_shared.Models.SensorType.Relay).Select(s => s.Id);
                foreach (var r in relays)
                {
                    _valveState.Add(r, false);
                }
            }


            if (_share.LastHumidity != null && _share.LastHumidity >= 30)
            {
                var valves = _valveState.Where(v => !v.Value);
                foreach (var v in valves)
                    {
                        _logger.LogInformation($"{DateTimeOffset.Now} ValveWorker: Opening Valve {v.Key}");
                        // open valve 
                        _valveState[v.Key] = true;
                    }
                
            } 
            else
            {
                
                
                var valves = _valveState.Where(v => !v.Value);
                foreach (var v in valves)
                    {
                        _logger.LogInformation($"{DateTimeOffset.Now} ValveWorker: Closing Valve {v.Key}");
                        // open valve 
                        _valveState[v.Key] = false;
                    }
            }


            await Task.Delay(5000, stoppingToken);
        }
    }
}
