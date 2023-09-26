namespace FolderSynchronization.Extensions;
public static class PathExtension
{
    public static string FormatFolderPath(this string folderPath) =>
        folderPath.Substring(folderPath.Length - 1) == "\\" ? folderPath : folderPath + "\\";

    public static IEnumerable<string> GetFileNames(this string[] filePathList)
    {
        foreach (var filePath in filePathList)
        {
            yield return Path.GetFileName(filePath);
        }
    }
}
