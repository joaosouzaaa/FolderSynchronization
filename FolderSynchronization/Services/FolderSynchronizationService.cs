using FolderSynchronization.Arguments;
using FolderSynchronization.Interfaces.Services;
using System.IO.Abstractions;

namespace FolderSynchronization.Services;
public sealed class FolderSynchronizationService : IFolderSynchronizationService
{
    private readonly IFileSystem _fileSystem;
    private readonly ILoggerService _loggerService;

    public FolderSynchronizationService(IFileSystem fileSystem, ILoggerService loggerService)
    {
        _fileSystem = fileSystem;
        _loggerService = loggerService;
    }

    public async Task SynchronizeFoldersAsync(SynchronizeFoldersArgument synchronizeFolders)
    {
        await _loggerService.LogMessageAsync(synchronizeFolders.FileLogPath, $"Folder synchronization worker started at: {DateTime.Now}");

        await CopyMissingFilesAsync(synchronizeFolders);

        await DeleteNonExistantFilesAsync(synchronizeFolders);
    }

    private async Task CopyMissingFilesAsync(SynchronizeFoldersArgument synchronizeFolders)
    {
        var sourceFolderPath = synchronizeFolders.SourceFolderPath;
        var destinationFolderPath = synchronizeFolders.DestinationFolderPath;
        var sourceFiles = _fileSystem.Directory.GetFiles(sourceFolderPath);
        var destinationFiles = _fileSystem.Directory.GetFiles(destinationFolderPath);

        var filesToAddList = GetFilesToAddList(sourceFiles, destinationFiles);
        foreach (var fileToAdd in filesToAddList)
        {
            var destinationPath = destinationFolderPath + fileToAdd;
            var fileToAddPath = sourceFolderPath + fileToAdd;
            _fileSystem.File.Copy(fileToAddPath, destinationPath);
            await _loggerService.LogMessageAsync(synchronizeFolders.FileLogPath, $"{fileToAdd} copied to {destinationFolderPath} - {DateTime.Now}");
        }
    }

    private List<string> GetFilesToAddList(string[] sourceFiles, string[] destinationFiles)
    {
        var sourceFileNameList = GetFileNames(sourceFiles);
        var destinationFileNameList = GetFileNames(destinationFiles);
        return sourceFileNameList.Except(destinationFileNameList).ToList();
    }

    private async Task DeleteNonExistantFilesAsync(SynchronizeFoldersArgument synchronizeFolders)
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
        var sourceFileNameList = GetFileNames(sourceFiles);
        var destinationFileNameList = GetFileNames(destinationFiles);

        return destinationFileNameList.Except(sourceFileNameList).ToList();
    }

    private IEnumerable<string> GetFileNames(string[] filePathList)
    {
        foreach (var filePath in filePathList)
        {
            yield return _fileSystem.Path.GetFileName(filePath);
        }
    }
}
