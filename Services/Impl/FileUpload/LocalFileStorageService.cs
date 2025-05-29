namespace portal.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(string basePath) => _basePath = basePath;

    public async Task<string> UploadAsync(Stream fileStream, string fileName)
    {
        Directory.CreateDirectory(_basePath);
        var safe = Path.GetFileName(fileName);
        var full = Path.Combine(_basePath, safe);

        await using var fs = File.Create(full);
        await fileStream.CopyToAsync(fs);
        return safe;
    }

    public Task DeleteAsync(string fileName)
    {
        var safe = Path.GetFileName(fileName);
        var full = Path.Combine(_basePath, safe);
        if (File.Exists(full))
            File.Delete(full);
        return Task.CompletedTask;
    }

    public Task<Stream> DownloadAsync(string fileName)
    {
        var safe = Path.GetFileName(fileName);
        var full = Path.Combine(_basePath, safe);
        if (!File.Exists(full))
            throw new FileNotFoundException($"Signature file not found: {safe}");
        Stream s = new FileStream(full, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(s);
    }

    public async Task<bool> ExistsAsync(string fileName)
    {
        var safe = Path.GetFileName(fileName);
        var full = Path.Combine(_basePath, safe);
        return File.Exists(full);
    }

    public Task<List<string>> ListFilesAsync(string prefix = "")
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> UploadMultipleAsync(
        List<(Stream fileStream, string remotePath)> files
    )
    {
        throw new NotImplementedException();
    }
}
