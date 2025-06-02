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

    public Task<string> ChangeFileNameAsync(string oldFileName, string newFileName)
    {
        var oldSafe = Path.GetFileName(oldFileName);
        var newSafe = Path.GetFileName(newFileName);

        var oldFullPath = Path.Combine(_basePath, oldSafe);
        var newFullPath = Path.Combine(_basePath, newSafe);

        if (!File.Exists(oldFullPath))
            throw new FileNotFoundException($"File not found: {oldSafe}");

        File.Move(oldFullPath, newFullPath);
        return Task.FromResult(newSafe);
    }

    public Task<string> MoveFileToAnotherLocationAsync(string oldLocation, string newLocation)
    {
        var oldFullPath = Path.Combine(_basePath, oldLocation);
        var newFullPath = Path.Combine(_basePath, newLocation);

        var newDir = Path.GetDirectoryName(newFullPath);
        if (!Directory.Exists(newDir))
        {
            Directory.CreateDirectory(newDir);
        }

        if (!File.Exists(oldFullPath))
            throw new FileNotFoundException($"File not found: {oldLocation}");

        File.Move(oldFullPath, newFullPath);
        return Task.FromResult(newLocation);
    }
}