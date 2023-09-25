using FolderSynchronization.Arguments;
using FolderSynchronization.Interfaces.Services;
using FolderSynchronization.Services;
using Moq;

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
        var timeInterval = 123;
        _inputServiceMock.Setup(i => i.GetTimeInterval(out timeInterval))
            .Returns(true);

        var sourceFolderPath = "sourcefolder";
        _inputServiceMock.Setup(i => i.GetSourceFolderPath(out sourceFolderPath))
            .Returns(true);

        var destinationFolderPath = "dest";
        _inputServiceMock.Setup(i => i.GetDestinationFolderPath(out destinationFolderPath))
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

    [Fact]
    public async Task ExecuteAsync_GetTimeIntervalIsInvalid_ReturnFalse()
    {
        // A
        _inputServiceMock.Setup(i => i.GetTimeInterval(out It.Ref<int>.IsAny))
            .Returns(false);

        // A
        var result = await _executableService.ExecuteAsync();

        // A
        _inputServiceMock.Verify(i => i.GetSourceFolderPath(out It.Ref<string>.IsAny), Times.Never());
        _inputServiceMock.Verify(i => i.GetDestinationFolderPath(out It.Ref<string>.IsAny), Times.Never());
        _inputServiceMock.Verify(i => i.GetFileLogPath(out It.Ref<string>.IsAny), Times.Never());
        _folderSynchronizationServiceMock.Verify(f => f.SynchronizeFoldersAsync(
            It.IsAny<SynchronizeFoldersArgument>()),
            Times.Never());

        Assert.False(result.IsSuccess);
        Assert.Null(result.TimeIntervalSeconds);
    }

    [Fact]
    public async Task ExecuteAsync_GetSourceFolderPathIsInvalid_ReturnsFalse()
    {
        // A
        _inputServiceMock.Setup(i => i.GetTimeInterval(out It.Ref<int>.IsAny))
            .Returns(true);

        _inputServiceMock.Setup(i => i.GetSourceFolderPath(out It.Ref<string>.IsAny))
            .Returns(false);

        // A
        var result = await _executableService.ExecuteAsync();

        // A
        _inputServiceMock.Verify(i => i.GetDestinationFolderPath(out It.Ref<string>.IsAny), Times.Never());
        _inputServiceMock.Verify(i => i.GetFileLogPath(out It.Ref<string>.IsAny), Times.Never());
        _folderSynchronizationServiceMock.Verify(f => f.SynchronizeFoldersAsync(
            It.IsAny<SynchronizeFoldersArgument>()),
            Times.Never());

        Assert.False(result.IsSuccess);
        Assert.Null(result.TimeIntervalSeconds);
    }

    [Fact]
    public async Task ExecuteAsync_GetDestinationFolderPathIsInvalid_ReturnsFalse()
    {
        // A
        _inputServiceMock.Setup(i => i.GetTimeInterval(out It.Ref<int>.IsAny))
            .Returns(true);

        _inputServiceMock.Setup(i => i.GetSourceFolderPath(out It.Ref<string>.IsAny))
            .Returns(true);

        _inputServiceMock.Setup(i => i.GetDestinationFolderPath(out It.Ref<string>.IsAny))
            .Returns(false);

        // A
        var result = await _executableService.ExecuteAsync();

        // A
        _inputServiceMock.Verify(i => i.GetFileLogPath(out It.Ref<string>.IsAny), Times.Never());
        _folderSynchronizationServiceMock.Verify(f => f.SynchronizeFoldersAsync(
            It.IsAny<SynchronizeFoldersArgument>()),
            Times.Never());

        Assert.False(result.IsSuccess);
        Assert.Null(result.TimeIntervalSeconds);
    }

    [Fact]
    public async Task ExecuteAsync_SourceFolder_Is_EqualTo_DestinationFolder_ReturnsFalse()
    {
        // A
        _inputServiceMock.Setup(i => i.GetTimeInterval(out It.Ref<int>.IsAny))
            .Returns(true);

        var sameFolderPath = "random";
        _inputServiceMock.Setup(i => i.GetSourceFolderPath(out sameFolderPath))
            .Returns(true);

        _inputServiceMock.Setup(i => i.GetDestinationFolderPath(out sameFolderPath))
            .Returns(true);

        // A
        var result = await _executableService.ExecuteAsync();

        // A
        _inputServiceMock.Verify(i => i.GetFileLogPath(out It.Ref<string>.IsAny), Times.Never());
        _folderSynchronizationServiceMock.Verify(f => f.SynchronizeFoldersAsync(
            It.IsAny<SynchronizeFoldersArgument>()),
            Times.Never());

        Assert.False(result.IsSuccess);
        Assert.Null(result.TimeIntervalSeconds);
    }

    [Fact]
    public async Task ExecuteAsync_GetFileLogPathIsInvalid_ReturnsFalse()
    {
        // A
        _inputServiceMock.Setup(i => i.GetTimeInterval(out It.Ref<int>.IsAny))
            .Returns(true);

        _inputServiceMock.Setup(i => i.GetSourceFolderPath(out It.Ref<string>.IsAny))
            .Returns(true);

        _inputServiceMock.Setup(i => i.GetDestinationFolderPath(out It.Ref<string>.IsAny))
            .Returns(true);

        _inputServiceMock.Setup(i => i.GetFileLogPath(out It.Ref<string>.IsAny))
            .Returns(false);

        // A
        var result = await _executableService.ExecuteAsync();

        // A
        _folderSynchronizationServiceMock.Verify(f => f.SynchronizeFoldersAsync(
            It.IsAny<SynchronizeFoldersArgument>()),
            Times.Never());

        Assert.False(result.IsSuccess);
        Assert.Null(result.TimeIntervalSeconds);
    }
}
