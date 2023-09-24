using FolderSynchronization.Arguments;
using FolderSynchronization.Interfaces.Services;

namespace FolderSynchronization;

public sealed class FolderSynchronizationWorker : BackgroundService
{
    private readonly IInputService _inputService;
    private readonly IFolderSynchronizationService _folderSynchronizationService;

    public FolderSynchronizationWorker(IInputService inputService, IFolderSynchronizationService folderSynchronizationService)
    {
        _inputService = inputService;
        _folderSynchronizationService = folderSynchronizationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!_inputService.GetTimeInterval(out var timeInterval))
                continue;

            if (!_inputService.GetSourceFolderPath(out var sourceFolderPath))
                continue;

            if (!_inputService.GetDestinationFolderPath(out var destinationFolderPath))
                continue;

            if (sourceFolderPath == destinationFolderPath)
            {
                Console.WriteLine("Source folder cannot be the same as destination folder.");
                continue;
            }

            if (!_inputService.GetFileLogPath(out var fileLogPath))
                continue;

            var synchronizeFolders = new SynchronizeFoldersArgument()
            {
                DestinationFolderPath = destinationFolderPath,
                FileLogPath = fileLogPath,
                SourceFolderPath = sourceFolderPath
            };

            await _folderSynchronizationService.SynchronizeFoldersAsync(synchronizeFolders);

            await Task.Delay(timeInterval * 1000, stoppingToken);
        }
    }
}
