using FolderSynchronization.Arguments;
using FolderSynchronization.Interfaces.Services;
using FolderSynchronization.Services;

namespace UnitTests.ServiceTests;
public sealed class ExecutableServiceTests
{
    private readonly Mock<IInputService> _inputServiceMock;
    private readonly Mock<IFolderSynchronizationService> _folderSynchronizationServiceMock;
    private readonly ExecutableService _executableService;

    public ExecutableServiceTests()
    {
        _inputServiceMock = new Mock<IInputService>();
        _folderSynchronizationServiceMock = new Mock<IFolderSynchronizationService>();
        _executableService = new ExecutableService(_inputServiceMock.Object, _folderSynchronizationServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_SuccessfullScenario()
    {
        // A
        _inputServiceMock.Setup(i => i.GetTimeInterval(out It.Ref<int>.IsAny))
            .Returns(true);

        var sourceFolderPath = "random source path";
        _inputServiceMock.Setup(i => i.GetSourceFolderPath(out sourceFolderPath))
            .Returns(true);

        var destinationFolderPath = "random destination path";
        _inputServiceMock.Setup(i => i.GetDestinationFolderPath(out destinationFolderPath))
            .Returns(true);

        var timeInterval = 123;
        _inputServiceMock.Setup(i => i.GetTimeInterval(out timeInterval))
            .Returns(true);
        _inputServiceMock.Setup(i => i.GetFileLogPath(out It.Ref<string>.IsAny))
            .Returns(true);

        // A
        var result = await _executableService.ExecuteAsync();

        // A
        _folderSynchronizationServiceMock.Verify(f => f.SynchronizeFoldersAsync(
            It.IsAny<SynchronizeFoldersArgument>()), 
            Times.Once());

        Assert.True(result.IsSuccess);
        Assert.Equal(result.TimeIntervalSeconds, timeInterval);
    }
}
