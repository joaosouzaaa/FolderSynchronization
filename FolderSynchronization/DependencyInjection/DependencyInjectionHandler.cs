using System.IO.Abstractions;

namespace FolderSynchronization.DependencyInjection;
public static class DependencyInjectionHandler
{
    public static void AddDependencyInjectionHandler(this IServiceCollection services)
    {
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddServicesDependencyInjection();
    }
}
