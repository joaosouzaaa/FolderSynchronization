using FolderSynchronization.Extensions;

namespace UnitTests.ExtensionTests;
public sealed class PathExtensionTests
{
    [Fact]
    public void FormatFolderPath_FormatsPath()
    {
        // A
        var folderPathToFormat = "C:\\path";

        // A
        var folderPathFormated = folderPathToFormat.FormatFolderPath();

        // A
        Assert.Equal(folderPathFormated, folderPathToFormat + "\\");
    }

    [Fact]
    public void FormatFolderPath_DoesNotFormathPath()
    {
        // A
        var folderPathToFormatAlreadyFormated = "C:\\path\\";

        // A
        var forlderPathFormated = folderPathToFormatAlreadyFormated.FormatFolderPath();

        // A
        Assert.Equal(forlderPathFormated, folderPathToFormatAlreadyFormated);
    }
}
