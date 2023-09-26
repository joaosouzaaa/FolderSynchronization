using FolderSynchronization.Arguments;

namespace FolderSynchronization.Interfaces.Services;
public interface ICopyFilesService
{
    Task CopyMissingFilesAsync(SynchronizeFoldersArgument synchronizeFolders);
}
