using FolderSynchronization;
using FolderSynchronization.DependencyInjection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDependencyInjectionHandler();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
