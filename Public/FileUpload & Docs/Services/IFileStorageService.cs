namespace portal.Services;

public interface IFileStorageService
{
    // Download - Upload operations
    Task<bool> AreExists(List<string> paths);
    Task<string> UploadAsync(Stream fileStream, string fileName);
    Task<Stream> DownloadAsync(string fileName);
    Task<List<string>> MultipleUploadAsync(
        IEnumerable<(Stream fileStream, string remotePath)> files
    );
    Task<Dictionary<string, MemoryStream>> MultipleDownloadAsync(IEnumerable<string> remotePaths);
    Task<string> ChangeFileNameAsync(string oldFileName, string newFileName);

    // Replace Files
    Task<bool> ReplaceFileAsync(string fileName, Stream newFileStream);

    // CRUD operations
    Task<bool> CopyAsync(IEnumerable<(string sourcePath, string destinationPath)> files);
    Task<bool> MoveAsync(IEnumerable<(string sourcePath, string destinationPath)> files);
    Task<bool> DeleteAsync(IEnumerable<string> filePaths);
    Task<object> FolderStructureAsync(string path = "");
    Task<object> FolderStructureAsync(string employeeMainId, string path = "");
}
