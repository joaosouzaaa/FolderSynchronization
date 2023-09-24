using FolderSynchronization.Arguments;

namespace FolderSynchronization.Interfaces.Services;
public interface IFolderSynchronizationService
{
    Task SynchronizeFoldersAsync(SynchronizeFoldersArgument synchronizeFolders);
}
