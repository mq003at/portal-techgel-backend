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
            var directory = Path.GetDirectoryName(remotePath)?.Replace('\\', '/');
            if (!string.IsNullOrEmpty(directory) && !client.Exists(directory))
            {
                client.CreateDirectory(directory);
            }

            fileStream.Position = 0;
            _logger.LogInformation("Uploading file to SFTP: {RemotePath}", remotePath);
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
}
