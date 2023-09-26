using FolderSynchronization.Interfaces.Services;
using FolderSynchronization.Services;

namespace FolderSynchronization.DependencyInjection;
public static class ServicesDependencyInjection
{
    public static void AddServicesDependencyInjection(this IServiceCollection services)
    {
        services.AddTransient<ICopyFilesService, CopyFilesService>();
        services.AddTransient<IDeleteFilesService, DeleteFilesService>();
        services.AddTransient<IExecutableService, ExecutableService>();
        services.AddTransient<IFolderSynchronizationService, FolderSynchronizationService>();
        services.AddTransient<IInputService, InputService>();
        services.AddTransient<ILoggerService, LoggerService>();
    }
}
