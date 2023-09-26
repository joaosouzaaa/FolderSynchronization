using FolderSynchronization.Arguments;
using FolderSynchronization.Extensions;
using FolderSynchronization.Interfaces.Services;
using System.IO.Abstractions;

namespace FolderSynchronization.Services;
public sealed class CopyFilesService : ICopyFilesService
{
    private readonly IFileSystem _fileSystem;
    private readonly ILoggerService _loggerService;

    public CopyFilesService(IFileSystem fileSystem, ILoggerService loggerService)
    {
        _fileSystem = fileSystem;
        _loggerService = loggerService;
    }

    public async Task CopyMissingFilesAsync(SynchronizeFoldersArgument synchronizeFolders)
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
        var sourceFileNameList = sourceFiles.GetFileNames();
        var destinationFileNameList = destinationFiles.GetFileNames();
        return sourceFileNameList.Except(destinationFileNameList).ToList();
    }
}
