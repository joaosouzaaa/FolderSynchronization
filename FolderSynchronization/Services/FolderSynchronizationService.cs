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

	public async Task SynchronizeFoldersAsync(string fileLogPath, string sourceFolderPath, string destinationFolderPath)
	{
        await _loggerService.LogMessageAsync(fileLogPath, $"Folder synchronization worker started at: {DateTime.Now}");

		destinationFolderPath = destinationFolderPath.Substring(destinationFolderPath.Length - 1) == "\\" ? destinationFolderPath : destinationFolderPath + "\\";
		var sourceFiles = _fileSystem.Directory.GetFiles(sourceFolderPath);
		var destinationFiles = _fileSystem.Directory.GetFiles(destinationFolderPath);

		var filesToAddList = sourceFiles.Except(destinationFiles).ToList();
		foreach (var fileToAdd in filesToAddList)
		{
            _fileSystem.File.Copy(fileToAdd, destinationFolderPath + _fileSystem.Path.GetFileName(fileToAdd));
		}
    }
}
