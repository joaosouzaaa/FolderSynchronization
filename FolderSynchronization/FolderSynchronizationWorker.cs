using FolderSynchronization.Interfaces.Services;

namespace FolderSynchronization;

public sealed class FolderSynchronizationWorker : BackgroundService
{
    private readonly IExecutableService _executableService;

    public FolderSynchronizationWorker(IExecutableService executableService)
    {
        _executableService = executableService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var synchronizeFoldersResult = await _executableService.ExecuteAsync();
            if (!synchronizeFoldersResult.IsSuccess)
                continue;

            await Task.Delay((int)synchronizeFoldersResult.TimeInterval * 1000, stoppingToken);
        }
    }
}
