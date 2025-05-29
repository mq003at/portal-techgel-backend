namespace portal.Services;

public interface IFileStorageService
{
    Task<List<string>> ListFilesAsync(string prefix = "");
    Task<string> UploadAsync(Stream fileStream, string fileName);
    Task DeleteAsync(string fileName);
    Task<Stream> DownloadAsync(string fileName);
    Task<bool> Exists(string fileName);
    Task<List<string>> UploadMultipleAsync(List<(Stream fileStream, string remotePath)> files);
}
