namespace FolderSynchronization.Interfaces.Services;
public interface ILoggerService
{
    Task LogMessageAsync(string fileLogPath, string message);
}
