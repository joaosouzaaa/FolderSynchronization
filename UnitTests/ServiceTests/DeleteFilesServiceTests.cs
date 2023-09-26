using FolderSynchronization.Arguments;
using FolderSynchronization.Interfaces.Services;
using FolderSynchronization.Services;
using Moq;
using System.IO.Abstractions;

namespace UnitTests.ServiceTests;
public sealed class DeleteFilesServiceTests
{
    private readonly Mock<IFileSystem> _fileSystemMock;
    private readonly Mock<ILoggerService> _loggerServiceMock;
    private readonly DeleteFilesService _deleteFilesService;

    public DeleteFilesServiceTests()
    {
        _fileSystemMock = new Mock<IFileSystem>();
        _loggerServiceMock = new Mock<ILoggerService>();
        _deleteFilesService = new DeleteFilesService(_fileSystemMock.Object, _loggerServiceMock.Object);
    }

    [Fact]
    public async Task DeleteNonExistantFilesAsync_SuccessfullScenario()
    {
        // A
        var synchronizeFolders = new SynchronizeFoldersArgument()
        {
            DestinationFolderPath = "random",
            FileLogPath = "a",
            SourceFolderPath = "path"
        };

        var sourceFileNameList = new string[]
        {
            "C:\\path\\file.txt",
            "C:\\path\\file1.txt",
            "C:\\path\\file56.txt"
        };
        var destinationFileNameList = new string[]
        {
             "C:\\path\\file568.txt",
             "C:\\path\\file3.txt",
             "C:\\path\\file56.txt"
        };
        _fileSystemMock.SetupSequence(f => f.Directory.GetFiles(It.IsAny<string>()))
            .Returns(sourceFileNameList)
            .Returns(destinationFileNameList);

        var countDeleteIsCalled = destinationFileNameList.Except(sourceFileNameList).ToList().Count;

        _fileSystemMock.Setup(f => f.File.Delete(It.IsAny<string>()));

        // A
        await _deleteFilesService.DeleteNonExistantFilesAsync(synchronizeFolders);

        // A
        _fileSystemMock.Verify(f => f.File.Delete(It.IsAny<string>()), Times.Exactly(countDeleteIsCalled));
        _loggerServiceMock.Verify(l => l.LogMessageAsync(It.IsAny<string>(), It.IsAny<string>()));
    }
}
