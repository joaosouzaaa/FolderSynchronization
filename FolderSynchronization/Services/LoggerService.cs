using FolderSynchronization.Interfaces.Services;

namespace FolderSynchronization.Services;
public sealed class LoggerService : ILoggerService
{
    private readonly ILogger<LoggerService> _logger;

	public LoggerService(ILogger<LoggerService> logger)
	{
		_logger = logger;
	}

	public async Task LogMessageAsync(string fileLogPath, string message)
	{
		_logger.LogInformation(message);

		using var writer = new StreamWriter(fileLogPath, true);
		await writer.WriteLineAsync(message);
    }
}
