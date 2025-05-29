using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class SignatureService : ISignatureService
{
    private readonly ApplicationDbContext _ctx;
    private readonly IFileStorageService _storage;
    private readonly IFileNameValidationService _validator;
    private readonly ILogger<SignatureService> _logger;
    private readonly SignatureOptions _sigOpts;
    private readonly string _publicBase;

    public SignatureService(
        ApplicationDbContext ctx,
        IFileStorageService storage,
        IFileNameValidationService validator,
        ILogger<SignatureService> logger,
        IOptions<SignatureOptions> sigOpts
    )
    {
        _ctx = ctx;
        _storage = storage;
        _validator = validator;
        _logger = logger;
        _sigOpts = sigOpts.Value;
        _publicBase = _sigOpts.PublicBaseUrl.TrimEnd('/');
    }

    public async Task<SignatureDTO> UploadAsync(UploadSignatureDTO dto)
    {
        // Ensure unique name
        var remotePath = $"{_sigOpts.StorageDir.TrimEnd('/')}/{dto.FileName}";
        await _validator.EnsureUniqueAsync(dto.FileName);
        _logger.LogInformation(
            "Uploading signature for Employee {Id} to {Path}",
            dto.EmployeeId,
            remotePath
        );

        await using var stream = dto.File.OpenReadStream();
        await _storage.UploadAsync(stream, remotePath);

        // Upsert DB
        var sig = new Signature { EmployeeId = dto.EmployeeId };
        _ctx.Signatures.Add(sig);
        await _ctx.SaveChangesAsync(); // to get Id

        sig.FileName = dto.FileName;
        sig.StoragePath = remotePath;
        sig.ContentType = dto.File.ContentType!;
        sig.FileSize = dto.File.Length;
        sig.UploadedAt = DateTime.UtcNow;
        _ctx.Signatures.Update(sig);
        await _ctx.SaveChangesAsync();

        return ToDto(sig);
    }

    public async Task<SignatureDTO> UploadAndReplaceAsync(UploadSignatureDTO dto)
    {
        var sig =
            await _ctx.Signatures.FirstOrDefaultAsync(s => s.EmployeeId == dto.EmployeeId)
            ?? throw new KeyNotFoundException($"No signature for employee {dto.EmployeeId}");

        var remotePath = $"{_sigOpts.StorageDir.TrimEnd('/')}/{sig.FileName}";
        _logger.LogInformation(
            "Replacing signature for Employee {Id} at {Path}",
            dto.EmployeeId,
            remotePath
        );

        await using var stream = dto.File.OpenReadStream();
        await _storage.UploadAsync(stream, remotePath);

        sig.ContentType = dto.File.ContentType!;
        sig.FileSize = dto.File.Length;
        sig.UploadedAt = DateTime.UtcNow;
        _ctx.Signatures.Update(sig);
        await _ctx.SaveChangesAsync();

        return ToDto(sig);
    }

    public async Task<SignatureDTO?> GetByEmployeeAsync(int employeeId)
    {
        var sig = await _ctx
            .Signatures.AsNoTracking()
            .FirstOrDefaultAsync(s => s.EmployeeId == employeeId);
        return sig == null ? null : ToDto(sig);
    }

    public async Task DeleteSignatureAsync(int employeeId)
    {
        var sig = await _ctx.Signatures.FirstOrDefaultAsync(s => s.EmployeeId == employeeId);
        if (sig == null)
            return;
        var remotePath = sig.StoragePath;
        _logger.LogInformation(
            "Deleting signature for Employee {Id} at {Path}",
            employeeId,
            remotePath
        );
        await _storage.DeleteAsync(remotePath);
        _ctx.Signatures.Remove(sig);
        await _ctx.SaveChangesAsync();
    }

    public async Task<Stream> GetSignatureStreamAsync(int employeeId)
    {
        var sig =
            await _ctx
                .Signatures.AsNoTracking()
                .FirstOrDefaultAsync(s => s.EmployeeId == employeeId)
            ?? throw new FileNotFoundException($"Signature for employee {employeeId} not found");
        return await _storage.DownloadAsync(sig.StoragePath);
    }

    private SignatureDTO ToDto(Signature sig) =>
        new()
        {
            Id = sig.Id,
            EmployeeId = sig.EmployeeId,
            FileName = sig.FileName,
            FileUrl = $"{_publicBase}/{Path.GetFileName(sig.StoragePath)}"
        };
}
