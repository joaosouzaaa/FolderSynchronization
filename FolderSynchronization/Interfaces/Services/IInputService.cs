namespace FolderSynchronization.Interfaces.Services;
public interface IInputService
{
    bool GetTimeInterval(out int timeInterval);
    bool GetSourceFolderPath(out string sourceFolderPath);
    bool GetDestinationFolderPath(out string destinationFolderPath);
    bool GetFileLogPath(out string fileLogPath);
}
