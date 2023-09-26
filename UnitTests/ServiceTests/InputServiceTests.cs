using FolderSynchronization.Extensions;
using FolderSynchronization.Services;
using Moq;
using System.IO.Abstractions;

namespace UnitTests.ServiceTests;
public sealed class InputServiceTests
{
    private readonly Mock<IFileSystem> _fileSystemMock;
    private readonly InputService _inputService;
    private readonly Mock<TextReader> _consoleMock;

    public InputServiceTests()
    {
        _fileSystemMock = new Mock<IFileSystem>();
        _inputService = new InputService(_fileSystemMock.Object);
        _consoleMock = new Mock<TextReader>();
    }

    [Fact]
    public void GetTimeInterval_SuccessfullScenario()
    {
        // A
        const int validInput = 10;

        _consoleMock.Setup(c => c.ReadLine())
            .Returns(validInput.ToString());
        Console.SetIn(_consoleMock.Object);

        // A
        var getTimeIntervalResult = _inputService.GetTimeInterval(out int timeInterval);

        // A
        Assert.True(getTimeIntervalResult);
        Assert.Equal(validInput, timeInterval);
    }

    [Theory]
    [InlineData("-10")]
    [InlineData("-5")]
    [InlineData("random")]
    public void GetTimeInterval_InvalidInput_ReturnsFalse(string invalidInput)
    {
        // A
        _consoleMock.Setup(c => c.ReadLine())
            .Returns(invalidInput);
        Console.SetIn(_consoleMock.Object);

        // A
        var getTimeIntervalResult = _inputService.GetTimeInterval(out int timeInterval);

        // A
        Assert.False(getTimeIntervalResult);
    }

    [Fact]
    public void GetFolderPath_SuccessfullScenario()
    {
        // A
        const string inputMessage = "text";
        const string validInput = "random";

        _consoleMock.Setup(c => c.ReadLine())
            .Returns(validInput);
        Console.SetIn(_consoleMock.Object);

        _fileSystemMock.Setup(f => f.Directory.Exists(It.IsAny<string>()))
            .Returns(true);

        // A
        var getFolderPathResult = _inputService.GetFolderPath(inputMessage, out string folderPath);

        // A
        Assert.True(getFolderPathResult);
        Assert.Equal(validInput.FormatFolderPath(), folderPath);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetFolderPath_NullOrEmptyInput_ReturnsFalse(string invalidInput)
    {
        // A
        const string inputMessage = "text";

        _consoleMock.Setup(c => c.ReadLine())
            .Returns(invalidInput);
        Console.SetIn(_consoleMock.Object);

        _fileSystemMock.Setup(f => f.Directory.Exists(It.IsAny<string>()))
            .Returns(true);

        // A
        var getFolderPathResult = _inputService.GetFolderPath(inputMessage, out string folderPath);

        // A
        _fileSystemMock.Verify(f => f.Directory.Exists(It.IsAny<string>()), Times.Never());
        Assert.False(getFolderPathResult);
    }

    [Fact]
    public void GetFolderPath_DirectoryDoesNotExist_ReturnsFalse()
    {
        // A
        const string inputMessage = "text";
        const string validInput = "random";

        _consoleMock.Setup(c => c.ReadLine())
            .Returns(validInput);
        Console.SetIn(_consoleMock.Object);

        _fileSystemMock.Setup(f => f.Directory.Exists(It.IsAny<string>()))
            .Returns(false);

        // A
        var getFolderPathResult = _inputService.GetFolderPath(inputMessage, out string folderPath);

        // A
        Assert.False(getFolderPathResult);
    }

    [Fact]
    public void GetFileLogPath_SuccessfullScenario()
    {
        // A
        const string validInput = "random";

        _consoleMock.Setup(c => c.ReadLine())
            .Returns(validInput);
        Console.SetIn(_consoleMock.Object);

        _fileSystemMock.Setup(f => f.File.Exists(It.IsAny<string>()))
            .Returns(true);

        const string validFileExtension = ".txt";
        _fileSystemMock.Setup(f => f.Path.GetExtension(It.IsAny<string>()))
            .Returns(validFileExtension);

        // A
        var getFileLogPathResult = _inputService.GetFileLogPath(out string fileLogPath);

        // A
        Assert.True(getFileLogPathResult);
        Assert.Equal(fileLogPath, validInput);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetFileLogPath_NullOrEmptyInput_ReturnsFalse(string invalidInput)
    {
        // A
        _consoleMock.Setup(c => c.ReadLine())
            .Returns(invalidInput);
        Console.SetIn(_consoleMock.Object);

        // A
        var getFileLogPathResult = _inputService.GetFileLogPath(out string fileLogPath);

        // A
        _fileSystemMock.Verify(f => f.File.Exists(It.IsAny<string>()), Times.Never());
        _fileSystemMock.Verify(f => f.Path.GetExtension(It.IsAny<string>()), Times.Never());
        Assert.False(getFileLogPathResult);
    }

    [Fact]
    public void GetFileLogPath_FileDoesNotExist_ReturnsFalse()
    {
        // A
        const string validInput = "random";

        _consoleMock.Setup(c => c.ReadLine())
            .Returns(validInput);
        Console.SetIn(_consoleMock.Object);

        _fileSystemMock.Setup(f => f.File.Exists(It.IsAny<string>()))
            .Returns(false);

        // A
        var getFileLogPathResult = _inputService.GetFileLogPath(out string fileLogPath);

        // A
        _fileSystemMock.Verify(f => f.Path.GetExtension(It.IsAny<string>()), Times.Never());
        Assert.False(getFileLogPathResult);
    }

    [Theory]
    [InlineData(".csv")]
    [InlineData(".xlsx")]
    public void GetFileLogPath_ExtensionInvalid_ReturnsFalse(string invalidExtension)
    {
        // A
        const string validInput = "random";

        _consoleMock.Setup(c => c.ReadLine())
            .Returns(validInput);
        Console.SetIn(_consoleMock.Object);

        _fileSystemMock.Setup(f => f.File.Exists(It.IsAny<string>()))
            .Returns(true);

        _fileSystemMock.Setup(f => f.Path.GetExtension(It.IsAny<string>()))
            .Returns(invalidExtension);

        // A
        var getFileLogPathResult = _inputService.GetFileLogPath(out string fileLogPath);

        // A
        Assert.False(getFileLogPathResult);
    }
}
