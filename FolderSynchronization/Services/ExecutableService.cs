using FolderSynchronization.Arguments;
using FolderSynchronization.Interfaces.Services;

namespace FolderSynchronization.Services;
public sealed class ExecutableService : IExecutableService
{
    private readonly IInputService _inputService;
    private readonly IFolderSynchronizationService _folderSynchronizationService;

    public ExecutableService(IInputService inputService, IFolderSynchronizationService folderSynchronizationService)
    {
        _inputService = inputService;
        _folderSynchronizationService = folderSynchronizationService;
    }

    public async Task<ExecuteAsyncResultArgument> ExecuteAsync()
    {
        var result = new ExecuteAsyncResultArgument()
        {
            IsSuccess = false
        };

        if (!_inputService.GetTimeInterval(out var timeInterval))
            return result;

        if (!_inputService.GetSourceFolderPath(out var sourceFolderPath))
            return result;

        if (!_inputService.GetDestinationFolderPath(out var destinationFolderPath))
            return result;

        if (sourceFolderPath == destinationFolderPath)
        {
            Console.WriteLine("Source folder cannot be the same as destination folder.");
            return result;
        }

        if (!_inputService.GetFileLogPath(out var fileLogPath))
            return result;

        var synchronizeFolders = new SynchronizeFoldersArgument()
        {
            DestinationFolderPath = destinationFolderPath,
            FileLogPath = fileLogPath,
            SourceFolderPath = sourceFolderPath
        };

        await _folderSynchronizationService.SynchronizeFoldersAsync(synchronizeFolders);

        result.IsSuccess = true;
        result.TimeIntervalSeconds = timeInterval;
        return result;
    }
}
