namespace FolderSynchronization.Interfaces.Services;
public interface IFolderSynchronizationService
{
    Task SynchronizeFoldersAsync(string fileLogPath, string sourceFolderPath, string destinationFolderPath);
}
