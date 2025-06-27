namespace portal.Services;

using Microsoft.Extensions.Options;
using portal.Options;
using Renci.SshNet;

public class SftpFileStorageService : IFileStorageService
{
    private readonly SftpOptions _opts;
    private readonly SemaphoreSlim _sync = new(1, 1);
    private readonly ILogger<SftpFileStorageService> _logger;

    public SftpFileStorageService(IOptions<SftpOptions> opts)
    {
        _opts = opts.Value;
        _logger = LoggerFactory
            .Create(builder =>
            {
                builder.AddConsole();
            })
            .CreateLogger<SftpFileStorageService>();
    }

    // List file trong remote directory
    public async Task<List<string>> ListFilesAsync(string remoteDirectory)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = new SftpClient(
                _opts.Host,
                _opts.Port,
                _opts.Username,
                _opts.Password
            );
            client.Connect();

            if (!client.Exists(remoteDirectory))
                throw new DirectoryNotFoundException(
                    $"Remote directory not found: {remoteDirectory}"
                );

            var files = client
                .ListDirectory(remoteDirectory)
                .Where(f => !f.IsDirectory && f.Name != "." && f.Name != "..")
                .Select(f => f.Name)
                .ToList();

            client.Disconnect();
            return files;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<string> ChangeFileNameAsync(string oldFileName, string newFileName)
    {
        _logger.LogInformation("Renaming file from {OldFileName} to {NewFileName}", oldFileName, newFileName);

        await _sync.WaitAsync();
        try
        {
            using var client = new SftpClient(
                _opts.Host,
                _opts.Port,
                _opts.Username,
                _opts.Password
            );

            client.Connect();

            if (!client.Exists(oldFileName))
            {
                throw new FileNotFoundException($"File not found: {oldFileName}");
            }

            client.RenameFile(oldFileName, newFileName);
            client.Disconnect();

            return newFileName;
        }
        finally
        {
            _sync.Release();
        }
    }

    // Upload a single file to remote path
    public async Task<string> UploadAsync(Stream fileStream, string remotePath)
    {
        _logger.LogInformation("Uploading file to SFTP: {RemotePath}", remotePath);
        await _sync.WaitAsync();
        try
        {
            using var client = new SftpClient(
                _opts.Host,
                _opts.Port,
                _opts.Username,
                _opts.Password
            );

            client.Connect();

            // // Get all directories
            // var directories = client.ListDirectory("/")
            //     .Where(f => f.IsDirectory && f.Name != "." && f.Name != "..")
            //     .Select(d => d.FullName)
            //     .ToList();

            // // list all directories
            // foreach (var dir in directories)
            // {
            //     _logger.LogInformation("Checking directory: {Directory}", dir);
            // }

            var directory = Path.GetDirectoryName(remotePath)?.Replace('\\', '/');
            if (!string.IsNullOrEmpty(directory) && !client.Exists(directory))
            {
                // _logger.LogInformation("Creating directory: {Directory}", directory);
                // client.CreateDirectory(directory);
                throw new DirectoryNotFoundException($"Remote directory not found: {directory}");
            }

            // Check if the file already exists
            if (client.Exists(remotePath))
            {
                _logger.LogWarning("File already exists at remote path: {RemotePath}", remotePath);
                throw new IOException($"File already exists at remote path: {remotePath}");
            }

            fileStream.Position = 0;
            _logger.LogInformation("Uploading file to Diretory: {Directory}", directory);
            _logger.LogInformation("File size: {Size} bytes", fileStream.Length);
            client.UploadFile(fileStream, remotePath, true);
            client.Disconnect();

            return remotePath;
        }
        finally
        {
            _sync.Release();
        }
    }

    // Upload multiple files to remote paths, return the new path to the files
    public async Task<List<string>> UploadMultipleAsync(
        List<(Stream fileStream, string remotePath)> files
    )
    {
        var uploadTasks = new List<Task<string>>();

        foreach (var (fileStream, remotePath) in files)
        {
            uploadTasks.Add(UploadAsync(fileStream, remotePath));
        }

        var results = await Task.WhenAll(uploadTasks);
        return results.ToList();
    }

    // Delete a file at remote path
    public async Task DeleteAsync(string remotePath)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = new SftpClient(
                _opts.Host,
                _opts.Port,
                _opts.Username,
                _opts.Password
            );

            client.Connect();
            if (client.Exists(remotePath))
                client.DeleteFile(remotePath);
            client.Disconnect();
        }
        finally
        {
            _sync.Release();
        }
    }

    // Download a file from remote path
    public async Task<Stream> DownloadAsync(string remotePath)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = new SftpClient(
                _opts.Host,
                _opts.Port,
                _opts.Username,
                _opts.Password
            );

            client.Connect();
            if (!client.Exists(remotePath))
                throw new FileNotFoundException($"Remote file not found: {remotePath}");

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

    // Download a file from remote path
    public async Task<bool> Exists(string remotePath)
    {
        await _sync.WaitAsync();
        try
        {
            using var client = new SftpClient(
                _opts.Host,
                _opts.Port,
                _opts.Username,
                _opts.Password
            );

            client.Connect();
            var exists = client.Exists(remotePath);
            client.Disconnect();
            return exists;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<string> MoveFileToAnotherLocationAsync(string oldLocation, string newLocation)
    {
        _logger.LogInformation("Moving file from {OldLocation} to {NewLocation}", oldLocation, newLocation);

        await _sync.WaitAsync();
        try
        {
            using var client = new SftpClient(
                _opts.Host,
                _opts.Port,
                _opts.Username,
                _opts.Password
            );

            client.Connect();

            // Ensure the source file exists
            if (!client.Exists(oldLocation))
            {
                throw new FileNotFoundException($"Source file not found: {oldLocation}");
            }

            // Ensure the target directory exists, create it if not
            var newDirectory = Path.GetDirectoryName(newLocation)?.Replace('\\', '/');
            if (!string.IsNullOrEmpty(newDirectory) && !client.Exists(newDirectory))
            {
                client.CreateDirectory(newDirectory);
            }

            // Perform the move operation
            client.RenameFile(oldLocation, newLocation);
            client.Disconnect();

            return newLocation;
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
        using var client = new SftpClient(_opts.Host, _opts.Port, _opts.Username, _opts.Password);
        client.Connect();

        // Check if file exists
        if (!client.Exists(fileName))
        {
            client.Disconnect();
            return false;
        }

        // Delete the old file
        client.DeleteFile(fileName);

        // Ensure parent directory exists (in case file is in a subfolder)
        var directory = Path.GetDirectoryName(fileName)?.Replace('\\', '/');
        if (!string.IsNullOrEmpty(directory) && !client.Exists(directory))
        {
            client.CreateDirectory(directory);
        }

        // Upload new file
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

}
