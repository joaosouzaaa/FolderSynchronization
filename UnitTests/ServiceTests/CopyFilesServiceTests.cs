using FolderSynchronization.Arguments;
using FolderSynchronization.Interfaces.Services;
using FolderSynchronization.Services;
using Moq;
using System.IO.Abstractions;

namespace UnitTests.ServiceTests;
public sealed class CopyFilesServiceTests
{
    private readonly Mock<IFileSystem> _fileSystemMock;
    private readonly Mock<ILoggerService> _loggerServiceMock;
    private readonly CopyFilesService _copyFilesService;

    public CopyFilesServiceTests()
    {
        _fileSystemMock = new Mock<IFileSystem>();
        _loggerServiceMock = new Mock<ILoggerService>();
        _copyFilesService = new CopyFilesService(_fileSystemMock.Object, _loggerServiceMock.Object);
    }

    [Fact]
    public async Task CopyMissingFilesAsync_SuccessfullScenario()
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
            "C:\\path\\file3.txt",
            "C:\\path\\file56.txt"
        };
        var destinationFileNameList = new string[]
        {
             "C:\\path\\file568.txt",
             "C:\\path\\file3.txt",
        };
        _fileSystemMock.SetupSequence(f => f.Directory.GetFiles(It.IsAny<string>()))
            .Returns(sourceFileNameList)
            .Returns(destinationFileNameList);

        var countCopyIsCalled = sourceFileNameList.Except(destinationFileNameList).ToList().Count;

        _fileSystemMock.Setup(f => f.File.Copy(It.IsAny<string>(), It.IsAny<string>()));

        // A
        await _copyFilesService.CopyMissingFilesAsync(synchronizeFolders);

        // A
        _fileSystemMock.Verify(f => f.File.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(countCopyIsCalled));
        _loggerServiceMock.Verify(l => l.LogMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(countCopyIsCalled));
    }
}
