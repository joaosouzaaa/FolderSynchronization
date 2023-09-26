namespace FolderSynchronization.Interfaces.Services;
public interface IInputService
{
    bool GetTimeInterval(out int timeInterval);
    bool GetFolderPath(string inputMessage, out string folderPath);
    bool GetFileLogPath(out string fileLogPath);
}
