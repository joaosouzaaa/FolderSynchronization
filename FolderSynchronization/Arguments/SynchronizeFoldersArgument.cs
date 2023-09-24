using FolderSynchronization.Extensions;

namespace FolderSynchronization.Arguments;
public sealed class SynchronizeFoldersArgument
{
    public required string FileLogPath { get; set; }
    public required string SourceFolderPath { get; set; }
    public required string DestinationFolderPath { get; set; }
}
