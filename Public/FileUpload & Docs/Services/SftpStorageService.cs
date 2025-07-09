namespace portal.Services;

using Microsoft.Extensions.Options;
using portal.Options;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;

public class SftpFileStorageService : IFileStorageService
{
    private readonly SftpOptions _opts;
    private readonly SemaphoreSlim _sync = new(1, 1);
    private readonly ILogger<SftpFileStorageService> _logger;

    public SftpFileStorageService(IOptions<SftpOptions> opts)
    {
        _opts = opts.Value;
        _logger = LoggerFactory
            .Create(builder => builder.AddConsole())
            .CreateLogger<SftpFileStorageService>();
    }

    public async Task<string> UploadAsync(Stream fileStream, string remotePath)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();
            client.Connect();

            var directory = Path.GetDirectoryName(remotePath)?.Replace("\\", "/");
            if (!string.IsNullOrEmpty(directory) && !client.Exists(directory))
            {
                client.CreateDirectory(directory);
            }

            fileStream.Position = 0;
            client.UploadFile(fileStream, remotePath, true);

            client.Disconnect();
            return remotePath;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<List<string>> UploadMultipleAsync(
        List<(Stream fileStream, string remotePath)> files
    )
    {
        var uploadedPaths = new List<string>();

        foreach (var (fileStream, remotePath) in files)
        {
            var path = await UploadAsync(fileStream, remotePath);
            uploadedPaths.Add(path);
        }

        return uploadedPaths;
    }

    public async Task<bool> DeleteAsync(IEnumerable<string> filePaths)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();
            client.Connect();

            foreach (var path in filePaths)
            {
                if (client.Exists(path))
                {
                    var attrs = client.GetAttributes(path);
                    if (attrs.IsDirectory)
                    {
                        DeleteDirectoryRecursive(client, path);
                    }
                    else
                    {
                        client.DeleteFile(path);
                    }
                }
                else
                {
                    throw new FileNotFoundException("Remote path not found: " + path);
                }
            }

            client.Disconnect();
            return true;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<Stream> DownloadAsync(string remotePath)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();
            client.Connect();

            if (!client.Exists(remotePath))
                throw new FileNotFoundException("Remote file not found: " + remotePath);

            using var sftpStream = client.OpenRead(remotePath);
            var memory = new MemoryStream();
            await sftpStream.CopyToAsync(memory);
            memory.Position = 0;

            client.Disconnect();
            return memory;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<bool> AreExists(List<string> paths)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();
            client.Connect();

            foreach (var path in paths)
            {
                if (!client.Exists(path))
                {
                    client.Disconnect();
                    return false;
                }
            }

            client.Disconnect();
            return true;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<bool> ReplaceFileAsync(string fileName, Stream newFileStream)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();
            client.Connect();

            if (client.Exists(fileName))
                client.DeleteFile(fileName);

            newFileStream.Position = 0;
            client.UploadFile(newFileStream, fileName, true);

            client.Disconnect();
            return true;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<string> ChangeFileNameAsync(string oldFileName, string newFileName)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();
            client.Connect();

            if (!client.Exists(oldFileName))
                throw new FileNotFoundException("File not found: " + oldFileName);

            client.RenameFile(oldFileName, newFileName);
            client.Disconnect();
            return newFileName;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<string> MoveFileToAnotherLocationAsync(string oldLocation, string newLocation)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();
            client.Connect();

            if (!client.Exists(oldLocation))
                throw new FileNotFoundException("Source file not found: " + oldLocation);

            var newDirectory = Path.GetDirectoryName(newLocation)?.Replace("\\", "/");
            if (!string.IsNullOrEmpty(newDirectory) && !client.Exists(newDirectory))
            {
                client.CreateDirectory(newDirectory);
            }

            client.RenameFile(oldLocation, newLocation);
            client.Disconnect();
            return newLocation;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<object> FolderStructureAsync(string initPath = "")
    {
        const string basePath = "/erp/documents";
        var normalizedPath = Path.Combine(basePath, initPath ?? "").Replace('\\', '/').TrimEnd('/');

        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();

            try
            {
                client.Connect();

                if (!client.Exists(normalizedPath))
                    throw new DirectoryNotFoundException($"Path not found: {normalizedPath}");

                var folderName =
                    Path.GetFileName(normalizedPath)
                    ?? normalizedPath.Split('/').LastOrDefault()
                    ?? "root";

                var structure = GetDirectoryStructure(client, normalizedPath);
                return new Dictionary<string, object> { [folderName] = structure };
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
            }
        }
        finally
        {
            _sync.Release();
        }
    }

    private SftpClient CreateClient() =>
        new(_opts.Host, _opts.Port, _opts.Username, _opts.Password);

    private void DeleteDirectoryRecursive(SftpClient client, string path)
    {
        foreach (var entry in client.ListDirectory(path))
        {
            if (entry.Name == "." || entry.Name == "..")
                continue;

            var fullPath = entry.FullName;
            if (entry.IsDirectory)
            {
                DeleteDirectoryRecursive(client, fullPath);
            }
            else
            {
                client.DeleteFile(fullPath);
            }
        }
        client.DeleteDirectory(path);
    }

    private object GetDirectoryStructure(SftpClient client, string path)
    {
        var result = new Dictionary<string, object>();

        IEnumerable<ISftpFile> entries;
        try
        {
            entries = client.ListDirectory(path);
        }
        catch (SftpPermissionDeniedException ex)
        {
            _logger.LogWarning("Permission denied accessing {Path}: {Message}", path, ex.Message);
            return "[Permission Denied]";
        }

        foreach (var entry in entries)
        {
            if (entry.Name is "." or "..")
                continue;

            var fullPath = entry.FullName;
            var entryName = entry.Name;

            if (entry.IsDirectory)
            {
                try
                {
                    result[entryName] = GetDirectoryStructure(client, fullPath);
                }
                catch (SftpPermissionDeniedException ex)
                {
                    _logger.LogWarning(
                        "Permission denied accessing {Path}: {Message}",
                        fullPath,
                        ex.Message
                    );
                    result[entryName] = "[Permission Denied]";
                }
            }
            else
            {
                result[entryName] = "file";
            }
        }

        return result;
    }

    public async Task<bool> CopyAsync(
        IEnumerable<(string sourcePath, string destinationPath)> items
    )
    {
        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();
            client.Connect();
            var missingSources = new List<string>();

            foreach (var (source, destination) in items)
            {
                if (!client.Exists(source))
                    missingSources.Add(source);
            }

            if (missingSources.Any())
                throw new FileNotFoundException(
                    "Missing source files: " + string.Join(", ", missingSources)
                );

            foreach (var (source, destination) in items)
            {
                using var sourceStream = client.OpenRead(source);
                var destDir = Path.GetDirectoryName(destination)?.Replace("\\", "/");
                if (!string.IsNullOrEmpty(destDir) && !client.Exists(destDir))
                {
                    client.CreateDirectory(destDir);
                }

                var memory = new MemoryStream();
                await sourceStream.CopyToAsync(memory);
                memory.Position = 0;
                client.UploadFile(memory, destination, true);
            }

            client.Disconnect();
            return true;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<bool> MoveAsync(
        IEnumerable<(string sourcePath, string destinationPath)> items
    )
    {
        await _sync.WaitAsync();
        try
        {
            using var client = CreateClient();
            client.Connect();

            foreach (var (source, destination) in items)
            {
                if (!client.Exists(source))
                    throw new FileNotFoundException("File not found: " + source);

                var destDir = Path.GetDirectoryName(destination)?.Replace("\\", "/");
                if (!string.IsNullOrEmpty(destDir) && !client.Exists(destDir))
                {
                    client.CreateDirectory(destDir);
                }

                client.RenameFile(source, destination);
            }

            client.Disconnect();
            return true;
        }
        finally
        {
            _sync.Release();
        }
    }
}
