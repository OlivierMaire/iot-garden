using iot_garden_server.Services;

namespace iot_garden_server;

public class Worker2 : BackgroundService
{
    private readonly ILogger<Worker2> _logger;
    private readonly TestService _service;

    public Worker2(ILogger<Worker2> logger, TestService service)
    {
        _logger = logger;
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // infinite loop
            

            _service.Value = " I am Service 2";
			_logger.LogInformation("Information - Worker2 running at: {time} {value}", DateTimeOffset.Now,  _service.Value);
			_logger.LogWarning("Warning - Worker2 running at: {time}", DateTimeOffset.Now);
			_logger.LogError("Error - Worker2 running at: {time}", DateTimeOffset.Now);
			_logger.LogCritical("Critical - Worker running2 at: {time}", DateTimeOffset.Now);
            await Task.Delay(5000, stoppingToken);
        }
    }
}
