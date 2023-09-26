using FolderSynchronization.Arguments;
using FolderSynchronization.Interfaces.Services;
using FolderSynchronization.Services;
using Moq;

namespace UnitTests.ServiceTests;
public sealed class FolderSynchronizationServiceTests
{
    private readonly Mock<ILoggerService> _loggerServiceMock;
    private readonly Mock<ICopyFilesService> _copyFilesServiceMock;
    private readonly Mock<IDeleteFilesService> _deleteFilesServiceMock;
    private readonly FolderSynchronizationService _folderSynchronizationService;

    public FolderSynchronizationServiceTests()
    {
        _loggerServiceMock = new Mock<ILoggerService>();
        _copyFilesServiceMock = new Mock<ICopyFilesService>();
        _deleteFilesServiceMock = new Mock<IDeleteFilesService>();
        _folderSynchronizationService = new FolderSynchronizationService(_loggerServiceMock.Object, _copyFilesServiceMock.Object, _deleteFilesServiceMock.Object);
    }

    [Fact]
    public async Task SynchronizeFoldersAsync_SuccessfullScenario()
    {
        // A
        var synchronizeFolders = new SynchronizeFoldersArgument()
        {
            DestinationFolderPath = "random",
            FileLogPath = "log",
            SourceFolderPath = "source"
        };

        // A
        await _folderSynchronizationService.SynchronizeFoldersAsync(synchronizeFolders);

        // A
        _loggerServiceMock.Verify(l => l.LogMessageAsync(It.Is<string>(l => l == synchronizeFolders.FileLogPath), It.IsAny<string>()), Times.Exactly(2));
        _copyFilesServiceMock.Verify(c => c.CopyMissingFilesAsync(It.IsAny<SynchronizeFoldersArgument>()), Times.Once());
        _deleteFilesServiceMock.Verify(d => d.DeleteNonExistantFilesAsync(It.IsAny<SynchronizeFoldersArgument>()), Times.Once());
    }
}
