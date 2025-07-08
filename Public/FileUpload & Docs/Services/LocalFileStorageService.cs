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

    public Task<bool> DeleteAsync(IEnumerable<string> filePaths)
    {
        return Task.Run(() =>
        {
            var missingPaths = new List<string>();

            foreach (var relativePath in filePaths)
            {
                var fullPath = Path.Combine(_basePath, relativePath);

                if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
                {
                    missingPaths.Add(relativePath);
                }
            }

            if (missingPaths.Any())
                throw new FileNotFoundException(
                    "Không tìm thấy các tệp hoặc thư mục: " + string.Join(", ", missingPaths)
                );

            foreach (var relativePath in filePaths)
            {
                var fullPath = Path.Combine(_basePath, relativePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                else if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath, recursive: true); // deletes contents too
                }
            }

            return true;
        });
    }

    public Task<Stream> DownloadAsync(string fileName)
    {
        var fileOnly = Path.GetFileName(fileName); // removes directory traversal risk
        var fullPath = Path.Combine(_basePath, fileOnly);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"File not found: {fileOnly}");

        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

        return Task.FromResult(stream);
    }

    public Task<bool> AreExists(List<string> paths)
    {
        foreach (var path in paths)
        {
            var fullPath = Path.Combine(_basePath, path);
            if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
            {
                return Task.FromResult(false);
            }
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

    public async Task<List<string>> UploadMultipleAsync(
        List<(Stream fileStream, string remotePath)> files
    )
    {
        var uploadedFiles = new List<string>();

        foreach (var (fileStream, remotePath) in files)
        {
            var safeFileName = Path.GetFileName(remotePath); // removes any directory traversal
            var fullPath = Path.Combine(_basePath, safeFileName);

            Directory.CreateDirectory(_basePath); // ensure target folder exists

            await using var fs = File.Create(fullPath); // overwrites if file already exists
            await fileStream.CopyToAsync(fs);

            uploadedFiles.Add(safeFileName);
        }

        return uploadedFiles;
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
        var combinedPath = string.IsNullOrWhiteSpace(path)
            ? _basePath
            : Path.GetFullPath(Path.Combine(_basePath, path));

        if (!combinedPath.StartsWith(_basePath))
            throw new UnauthorizedAccessException("Access to this path is not allowed.");

        if (!Directory.Exists(combinedPath))
            throw new DirectoryNotFoundException($"Directory not found: {combinedPath}");

        var structure = GetDirectoryStructure(combinedPath);
        return Task.FromResult((object)structure); // can replace object with typed model
    }

    private object GetDirectoryStructure(string currentPath)
    {
        var result = new Dictionary<string, object>();

        var directories = Directory.GetDirectories(currentPath);
        foreach (var dir in directories)
        {
            var name = Path.GetFileName(dir);
            result[name] = GetDirectoryStructure(dir);
        }

        var files = Directory.GetFiles(currentPath);
        foreach (var file in files)
        {
            var name = Path.GetFileName(file);
            result[name] = "file";
        }

        return result;
    }
}
