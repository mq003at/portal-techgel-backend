using System.Text.RegularExpressions;
namespace portal.Helpers;

public static class FileNameHelper
{
    // Replace spaces with dashes, remove invalid characters, etc.
    public static string SanitizeFileName(string fileName)
    {
        // Replace spaces with dashes
        fileName = fileName.Replace(" ", "-");

        // Remove invalid characters
        fileName = Regex.Replace(fileName, "[^a-zA-Z0-9\\-_.]+", "");

        return fileName;
    }

    // Replace spaces with %20 for URL-safe filenames
    public static string EncodeForUrl(string fileName)
    {
        return Uri.EscapeDataString(fileName);
    }

    // Restore file name from URL
    public static string DecodeFromUrl(string encodedFileName)
    {
        return Uri.UnescapeDataString(encodedFileName);
    }

}