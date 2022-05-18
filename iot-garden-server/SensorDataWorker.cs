using iot_garden_server.Services;
namespace iot_garden_server;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly TestService _service;

    public Worker(ILogger<Worker> logger, TestService service)
    {
        _logger = logger;
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // infinite loop
            



			_logger.LogInformation("Information - Worker running at: {time} {value}", DateTimeOffset.Now,  _service.Value);
			_logger.LogWarning("Warning - Worker running at: {time}", DateTimeOffset.Now);
			_logger.LogError("Error - Worker running at: {time}", DateTimeOffset.Now);
			_logger.LogCritical("Critical - Worker running at: {time}", DateTimeOffset.Now);
             _service.Value = " I am Service 1";
            await Task.Delay(1000, stoppingToken);
        }
    }
}
