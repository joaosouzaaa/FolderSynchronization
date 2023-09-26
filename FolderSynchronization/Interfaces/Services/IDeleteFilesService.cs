using FolderSynchronization.Arguments;

namespace FolderSynchronization.Interfaces.Services;
public interface IDeleteFilesService
{
    Task DeleteNonExistantFilesAsync(SynchronizeFoldersArgument synchronizeFolders);
}
