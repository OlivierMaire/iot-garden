using iot_garden_server.Services;
using iot_garden_server;
using iot_garden_server.Workers;
using iot_garden_shared.Services;

IHost host = Host.CreateDefaultBuilder(args)
	.UseSystemd()
    .ConfigureServices(services =>
    {
        services.AddHostedService<TimedSensorSettingWorker>();
        services.AddHostedService<TimedDataUploadWorker>();
        services.AddHostedService<ValveWorker>();
        // services.AddHostedService<Worker2>();
        services.AddSingleton<ShareService>();
        services.AddSingleton<IFirestoreService, FirestoreService>();
        services.AddSingleton<SettingService>();
        services.AddSingleton<DataService>();
    })
    .Build();

await host.RunAsync();
