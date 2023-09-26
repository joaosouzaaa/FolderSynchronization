using FolderSynchronization.Extensions;
using FolderSynchronization.Interfaces.Services;
using System.IO.Abstractions;

namespace FolderSynchronization.Services;
public sealed class InputService : IInputService
{
    private readonly IFileSystem _fileSystem;

    public InputService(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public bool GetTimeInterval(out int timeInterval)
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

    public bool GetFolderPath(string inputMessage, out string folderPath)
    {
        Console.WriteLine(inputMessage);
        folderPath = Console.ReadLine();

        if (string.IsNullOrEmpty(folderPath))
        {
            Console.WriteLine(inputMessage);
            return false;
        }

        if (!_fileSystem.Directory.Exists(folderPath))
        {
            Console.WriteLine("The path for the folder provided does not exist.");
            return false;
        }

        folderPath = folderPath.FormatFolderPath();

        return true;
    }

    public bool GetFileLogPath(out string fileLogPath)
    {
        const string inputMessage = "Please provide txt file which logs are going to be logged.";
        Console.WriteLine(inputMessage);
        fileLogPath = Console.ReadLine();

        if (string.IsNullOrEmpty(fileLogPath))
        {
            Console.WriteLine(inputMessage);
            return false;
        }

        if (!_fileSystem.File.Exists(fileLogPath))
        {
            Console.WriteLine("The path for the file provided does not exist.");
            return false;
        }

        var fileExtension = _fileSystem.Path.GetExtension(fileLogPath);

        const string txtFileExtension = ".txt";
        if (fileExtension is not txtFileExtension)
        {
            Console.WriteLine("The file extension has to be a text file (.txt).");
            return false;
        }

        return true;
    }
}
