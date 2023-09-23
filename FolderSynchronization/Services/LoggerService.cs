using FolderSynchronization.Interfaces.Services;
using System.IO.Abstractions;

namespace FolderSynchronization.Services;
public sealed class LoggerService : ILoggerService
{
    private readonly ILogger<LoggerService> _logger;
	private readonly IFileSystem _fileSystem;

	public LoggerService(ILogger<LoggerService> logger, IFileSystem fileSystem)
	{
		_logger = logger;
		_fileSystem = fileSystem;
	}

	public async Task LogMessageAsync(string fileLogPath, string message)
	{
		_logger.LogInformation(message);

		await _fileSystem.File.AppendAllTextAsync(fileLogPath, message);
    }
}
