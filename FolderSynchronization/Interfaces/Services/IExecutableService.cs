using FolderSynchronization.Arguments;

namespace FolderSynchronization.Interfaces.Services;
public interface IExecutableService
{
    Task<ExecuteAsyncResultArgument> ExecuteAsync();
}
