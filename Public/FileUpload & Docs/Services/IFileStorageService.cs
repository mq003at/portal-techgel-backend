namespace portal.Services;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName);
    Task<Stream> DownloadAsync(string fileName);
    Task<bool> AreExists(List<string> paths);
    Task<List<string>> UploadMultipleAsync(List<(Stream fileStream, string remotePath)> files);
    Task<string> ChangeFileNameAsync(string oldFileName, string newFileName);

    Task<bool> ReplaceFileAsync(string fileName, Stream newFileStream);

    // CRUD operations
    Task<bool> CopyAsync(IEnumerable<(string sourcePath, string destinationPath)> files);
    Task<bool> MoveAsync(IEnumerable<(string sourcePath, string destinationPath)> files);
    Task<bool> DeleteAsync(IEnumerable<string> filePaths);
    Task<object> FolderStructureAsync(string path = "");
}
