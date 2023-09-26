using FolderSynchronization.Arguments;
using FolderSynchronization.Extensions;
using FolderSynchronization.Interfaces.Services;
using System.IO.Abstractions;

namespace FolderSynchronization.Services;
public sealed class DeleteFilesService : IDeleteFilesService
{
    private readonly IFileSystem _fileSystem;
    private readonly ILoggerService _loggerService;

    public DeleteFilesService(IFileSystem fileSystem, ILoggerService loggerService)
    {
        _fileSystem = fileSystem;
        _loggerService = loggerService;
    }

    public async Task DeleteNonExistantFilesAsync(SynchronizeFoldersArgument synchronizeFolders)
    {
        var destinationFolderPath = synchronizeFolders.DestinationFolderPath;
        var sourceFiles = _fileSystem.Directory.GetFiles(synchronizeFolders.SourceFolderPath);
        var destinationFiles = _fileSystem.Directory.GetFiles(destinationFolderPath);

        var filesToDeleteList = GetFilesToDeleteList(sourceFiles, destinationFiles);
        foreach (var fileToDelete in filesToDeleteList)
        {
            var fileToDeletePath = destinationFolderPath + fileToDelete;
            _fileSystem.File.Delete(fileToDeletePath);
            await _loggerService.LogMessageAsync(synchronizeFolders.FileLogPath, $"{fileToDelete} deleted from {destinationFolderPath} - {DateTime.Now}");
        }
    }

    private List<string> GetFilesToDeleteList(string[] sourceFiles, string[] destinationFiles)
    {
        var sourceFileNameList = sourceFiles.GetFileNames();
        var destinationFileNameList = destinationFiles.GetFileNames();

        return destinationFileNameList.Except(sourceFileNameList).ToList();
    }
}
