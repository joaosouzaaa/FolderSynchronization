using FolderSynchronization.Interfaces.Services;
using FolderSynchronization.Services;

namespace FolderSynchronization.DependencyInjection;
public static class ServicesDependencyInjection
{
    public static void AddServicesDependencyInjection(this IServiceCollection services)
    {
        services.AddTransient<IFolderSynchronizationService, FolderSynchronizationService>();
        services.AddTransient<ILoggerService, LoggerService>();
    }
}
