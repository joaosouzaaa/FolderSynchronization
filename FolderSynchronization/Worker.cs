using FolderSynchronization.Interfaces.Services;
using System.IO.Abstractions;

namespace FolderSynchronization;

public class Worker : BackgroundService
{
    private readonly IFileSystem _fileSystem;
    private readonly IFolderSynchronizationService _folderSynchronizationService;

    private const string _pathDoesNotExistLog = "The path for the folder provided does not exist.";

    public Worker(IFileSystem fileSystem, IFolderSynchronizationService folderSynchronizationService)
    {
        _fileSystem = fileSystem;
        _folderSynchronizationService = folderSynchronizationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!GetTimeInterval(out var timeInterval))
                continue;

            if (!GetSourceFolderPath(out var sourceFolderPath))
                continue;

            if (!GetDestinationFolderPath(out var destinationFolderPath))
                continue;

            if (sourceFolderPath == destinationFolderPath)
            {
                Console.WriteLine("Source folder cannot be the same as destination folder.");
                continue;
            }

            if (!GetFileLogPath(out var fileLogPath))
                continue;

            await _folderSynchronizationService.SynchronizeFoldersAsync(fileLogPath, sourceFolderPath, destinationFolderPath);
            
            await Task.Delay(timeInterval * 1000, stoppingToken);
        }
    }

    private bool GetTimeInterval(out int timeInterval)
    {
        Console.WriteLine("Please provide in seconds the time interval which you want to set for this job.");

        if (int.TryParse(Console.ReadLine(), out timeInterval) && timeInterval > 0)
        {
            return true;
        }
        else
        {
            Console.WriteLine("Time informed was not in a correct format.");
            return false;
        }
    }

    private bool GetSourceFolderPath(out string sourceFolderPath)
    {
        Console.WriteLine("Please provide the source folder path which the info gonna be synchronized.");
        sourceFolderPath = Console.ReadLine();

        return IsFolderPathValid(sourceFolderPath);
    }

    private bool GetDestinationFolderPath(out string destinationFolderPath)
    {
        Console.WriteLine("Please provide the destination folder path which the info gonna be synchronized.");
        destinationFolderPath = Console.ReadLine();

        return IsFolderPathValid(destinationFolderPath);
    }

    private bool IsFolderPathValid(string folderPath)
    {
        if (!_fileSystem.Directory.Exists(folderPath))
        {
            Console.WriteLine(_pathDoesNotExistLog);
            return false;
        }

        return true;
    }

    private bool GetFileLogPath(out string fileLogPath)
    {
        Console.WriteLine("Please provide txt file which logs are going to be logged.");
        fileLogPath = Console.ReadLine();

        if (!_fileSystem.File.Exists(fileLogPath))
        {
            Console.WriteLine("The path for the file provided does not exist.");
            return false;
        }

        var fileExtension = _fileSystem.Path.GetExtension(fileLogPath);

        const string txtFileExtension = ".txt";
        if(fileExtension is not txtFileExtension)
        {
            Console.WriteLine("The file extension has to be a text file (.txt).");
            return false;
        }

        return true;
    }
}
