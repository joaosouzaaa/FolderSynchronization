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

            const int milisecondsMultiplier = 1000;
            await Task.Delay((int)synchronizeFoldersResult.TimeIntervalSeconds * milisecondsMultiplier, stoppingToken);
        }
    }
}
