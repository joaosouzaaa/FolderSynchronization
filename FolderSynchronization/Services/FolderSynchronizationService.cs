using FolderSynchronization.Arguments;
using FolderSynchronization.Interfaces.Services;

namespace FolderSynchronization.Services;
public sealed class FolderSynchronizationService : IFolderSynchronizationService
{
    private readonly ILoggerService _loggerService;
    private readonly ICopyFilesService _copyFilesService;
    private readonly IDeleteFilesService _deleteFilesService;

    public FolderSynchronizationService(ILoggerService loggerService, ICopyFilesService copyFilesService, 
                                        IDeleteFilesService deleteFilesService)
    {
        _loggerService = loggerService;
        _copyFilesService = copyFilesService;
        _deleteFilesService = deleteFilesService;
    }

    public async Task SynchronizeFoldersAsync(SynchronizeFoldersArgument synchronizeFolders)
    {
        await _loggerService.LogMessageAsync(synchronizeFolders.FileLogPath, $"Folder synchronization worker started at: {DateTime.Now}");

        await _copyFilesService.CopyMissingFilesAsync(synchronizeFolders);

        await _deleteFilesService.DeleteNonExistantFilesAsync(synchronizeFolders);

        await _loggerService.LogMessageAsync(synchronizeFolders.FileLogPath, $"Folder synchronization worker ended at: {DateTime.Now}");
    }
}
