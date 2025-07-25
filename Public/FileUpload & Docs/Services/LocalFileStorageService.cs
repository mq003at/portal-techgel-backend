namespace portal.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;
    private readonly string _erpSubPath = "erp/documents";
    private readonly ILogger<LocalFileStorageService> _logger;

    public LocalFileStorageService(string basePath, ILogger<LocalFileStorageService> logger)
    {
        // Normalize to absolute path
        _basePath = Path.IsPathRooted(basePath)
            ? basePath
            : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, basePath));

        _logger = logger;

        _logger.LogWarning("Resolved file storage path: {ResolvedPath}", _basePath);
    }

    private string GetFullPath(string relativePath)
    {
        var safePath = relativePath.Replace('\\', '/').TrimStart('/');
        _logger.LogInformation(
            "Getting full path for: {SafePath}",
            Path.Combine(_basePath, safePath)
        );
        return Path.Combine(_basePath, safePath);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName)
    {
        var full = GetFullPath(fileName);
        var dir = Path.GetDirectoryName(full);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        await using var fs = File.Create(full);
        await fileStream.CopyToAsync(fs);
        return fileName.Replace('\\', '/');
    }

    public Task<bool> DeleteAsync(IEnumerable<string> filePaths)
    {
        return Task.Run(() =>
        {
            var missing = new List<string>();
            foreach (var rel in filePaths)
            {
                var full = GetFullPath(rel);
                if (!File.Exists(full) && !Directory.Exists(full))
                    missing.Add(rel);
            }

            if (missing.Any())
                throw new FileNotFoundException("Không tìm thấy: " + string.Join(", ", missing));

            foreach (var rel in filePaths)
            {
                var full = GetFullPath(rel);
                if (File.Exists(full))
                    File.Delete(full);
                else if (Directory.Exists(full))
                    Directory.Delete(full, recursive: true);
            }

            return true;
        });
    }

    public Task<Stream> DownloadAsync(string fileName)
    {
        var fullPath = GetFullPath(fileName);
        _logger.LogInformation("Downloading file: {FileName} from {FullPath}", fileName, fullPath);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"File not found: {fileName}");

        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(stream);
    }

    public Task<Dictionary<string, MemoryStream>> MultipleDownloadAsync(IEnumerable<string> paths)
    {
        return Task.Run(() =>
        {
            var result = new Dictionary<string, MemoryStream>();
            foreach (var rel in paths)
            {
                var full = GetFullPath(rel);
                if (!File.Exists(full))
                    throw new FileNotFoundException($"File not found: {rel}");

                var ms = new MemoryStream();
                using var fs = new FileStream(full, FileMode.Open, FileAccess.Read);
                fs.CopyTo(ms);
                ms.Position = 0;
                result[rel.Replace('\\', '/')] = ms;
            }
            return result;
        });
    }

    public Task<bool> AreExists(List<string> paths)
    {
        foreach (var path in paths)
        {
            var full = GetFullPath(path);
            if (!File.Exists(full) && !Directory.Exists(full))
                return Task.FromResult(false);
        }
        return Task.FromResult(true);
    }

    public Task<bool> MoveAsync(IEnumerable<(string sourcePath, string destinationPath)> items)
    {
        return Task.Run(() =>
        {
            var filesToMove = new List<(string source, string destination)>();
            var missingSources = new List<string>();
            var duplicateDestinations = new List<string>();

            foreach (var (sourceRel, destRel) in items)
            {
                var sourceFull = Path.Combine(_basePath, sourceRel);
                var destFull = Path.Combine(_basePath, destRel);

                if (File.Exists(sourceFull))
                {
                    var fileName = Path.GetFileName(sourceFull);
                    var destFile = Path.Combine(destFull, fileName);

                    if (File.Exists(destFile))
                        duplicateDestinations.Add(destFile);
                    else
                        filesToMove.Add((sourceFull, destFile));
                }
                else if (Directory.Exists(sourceFull))
                {
                    foreach (
                        var file in Directory.GetFiles(sourceFull, "*", SearchOption.AllDirectories)
                    )
                    {
                        var relativePath = Path.GetRelativePath(sourceFull, file);
                        var destFile = Path.Combine(destFull, relativePath);
                        var srcFile = file;

                        if (File.Exists(destFile))
                            duplicateDestinations.Add(destFile);
                        else
                            filesToMove.Add((srcFile, destFile));
                    }
                }
                else
                {
                    missingSources.Add(sourceRel);
                }
            }

            if (missingSources.Any())
                throw new FileNotFoundException(
                    "Không tìm thấy các nguồn: " + string.Join(", ", missingSources)
                );

            if (duplicateDestinations.Any())
                throw new IOException(
                    "Các tệp đích đã tồn tại: " + string.Join(", ", duplicateDestinations)
                );

            foreach (var (sourceFile, destFile) in filesToMove)
            {
                var destDir = Path.GetDirectoryName(destFile)!;
                Directory.CreateDirectory(destDir);
                File.Move(sourceFile, destFile); // ✅ Move instead of copy
            }

            return true;
        });
    }

    public Task<bool> CopyAsync(IEnumerable<(string sourcePath, string destinationPath)> items)
    {
        return Task.Run(() =>
        {
            var filesToCopy = new List<(string source, string destination)>();
            var missingSources = new List<string>();
            var duplicateDestinations = new List<string>();

            foreach (var (sourceRel, destRel) in items)
            {
                var sourceFull = Path.Combine(_basePath, sourceRel);
                var destFull = Path.Combine(_basePath, destRel);

                if (File.Exists(sourceFull))
                {
                    var fileName = Path.GetFileName(sourceFull);
                    var destFile = Path.Combine(destFull, fileName);

                    if (File.Exists(destFile))
                        duplicateDestinations.Add(destFile);
                    else
                        filesToCopy.Add((sourceFull, destFile));
                }
                else if (Directory.Exists(sourceFull))
                {
                    foreach (
                        var file in Directory.GetFiles(sourceFull, "*", SearchOption.AllDirectories)
                    )
                    {
                        var relativePath = Path.GetRelativePath(sourceFull, file);
                        var destFile = Path.Combine(destFull, relativePath);
                        var srcFile = file;

                        if (File.Exists(destFile))
                            duplicateDestinations.Add(destFile);
                        else
                            filesToCopy.Add((srcFile, destFile));
                    }
                }
                else
                {
                    missingSources.Add(sourceRel);
                }
            }

            if (missingSources.Any())
                throw new FileNotFoundException(
                    "Không tìm thấy các nguồn: " + string.Join(", ", missingSources)
                );

            if (duplicateDestinations.Any())
                throw new IOException(
                    "Các tệp đích đã tồn tại: " + string.Join(", ", duplicateDestinations)
                );

            foreach (var (sourceFile, destFile) in filesToCopy)
            {
                var destDir = Path.GetDirectoryName(destFile)!;
                Directory.CreateDirectory(destDir);
                File.Copy(sourceFile, destFile);
            }

            return true;
        });
    }

    public Task<List<string>> MultipleUploadAsync(
        IEnumerable<(Stream fileStream, string remotePath)> files
    )
    {
        return Task.Run(async () =>
        {
            var savedPaths = new List<string>();

            foreach (var (stream, remotePath) in files)
            {
                var safePath = remotePath.Replace('\\', '/');
                var fullPath = Path.Combine(_basePath, safePath);

                var directory = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrWhiteSpace(directory))
                    Directory.CreateDirectory(directory);

                stream.Position = 0;
                await using var fs = File.Create(fullPath); // will overwrite
                await stream.CopyToAsync(fs);

                savedPaths.Add(safePath);
            }

            return savedPaths;
        });
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

        string newDir = Path.GetDirectoryName(newFullPath) ?? string.Empty;
        if (!string.IsNullOrEmpty(newDir) && !Directory.Exists(newDir))
        {
            Directory.CreateDirectory(newDir);
        }

        if (!File.Exists(oldFullPath))
            throw new FileNotFoundException($"File not found: {oldLocation}");

        File.Move(oldFullPath, newFullPath);
        return Task.FromResult(newLocation);
    }

    public async Task<bool> ReplaceFileAsync(string fileName, Stream newFileStream)
    {
        var safe = Path.GetFileName(fileName);
        var full = Path.Combine(_basePath, safe);

        // Check if the file exists before replacing
        if (!File.Exists(full))
            return false;

        // Delete the old file
        File.Delete(full);

        // Upload the new file
        Directory.CreateDirectory(_basePath); // Ensure directory still exists
        await using var fs = File.Create(full);
        await newFileStream.CopyToAsync(fs);

        return true;
    }

    public Task<object> FolderStructureAsync(string path = "")
    {
        var combined = Path.GetFullPath(Path.Combine(_basePath, _erpSubPath, path ?? ""));

        if (!combined.StartsWith(Path.Combine(_basePath, _erpSubPath)))
            throw new UnauthorizedAccessException("Access denied");

        if (!Directory.Exists(combined))
            throw new DirectoryNotFoundException($"Not found: {combined}");

        var structure = GetDirectoryStructure(combined);
        return Task.FromResult((object)structure);
    }

    private object GetDirectoryStructure(string currentPath)
    {
        var result = new Dictionary<string, object>();
        foreach (var dir in Directory.GetDirectories(currentPath))
        {
            var name = Path.GetFileName(dir);
            result[name] = GetDirectoryStructure(dir);
        }
        foreach (var file in Directory.GetFiles(currentPath))
        {
            result[Path.GetFileName(file)] = "file";
        }
        return result;
    }

    public Task<object> FolderStructureAsync(string employeeMainId, string path = "")
    {
        throw new NotImplementedException();
    }
}
