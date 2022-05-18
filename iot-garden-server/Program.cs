using iot_garden_server.Services;
using iot_garden_server;

IHost host = Host.CreateDefaultBuilder(args)
	.UseSystemd()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHostedService<Worker2>();
        services.AddSingleton<TestService>();
    })
    .Build();

await host.RunAsync();
