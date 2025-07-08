using Microsoft.EntityFrameworkCore;
using portal.Db;

namespace portal.Services;

public interface IFileNameValidationService
{
    /// <summary>
    /// Throws if any signature record or storage file already uses this filename.
    /// </summary>
    Task EnsureUniqueAsync(string fileName);
}

public class FileNameValidationService : IFileNameValidationService
{
    private readonly ApplicationDbContext _ctx;
    private readonly IFileStorageService _storage;

    public FileNameValidationService(ApplicationDbContext ctx, IFileStorageService storage)
    {
        _ctx = ctx;
        _storage = storage;
    }

    public async Task EnsureUniqueAsync(string fileName)
    {
        var inDb = await _ctx.Signatures.AnyAsync(s => s.FileName == fileName);
        var onDisk = await _storage.AreExists(new List<string> { fileName });
        if (inDb || onDisk)
            throw new InvalidOperationException($"Filename '{fileName}' is already taken.");
    }
}
