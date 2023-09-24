namespace FolderSynchronization.Extensions;
public static class PathExtension
{
    public static string FormatFolderPath(this string folderPath) =>
        folderPath.Substring(folderPath.Length - 1) == "\\" ? folderPath : folderPath + "\\";
}
