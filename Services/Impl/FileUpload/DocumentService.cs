using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;
using portal.Options;
using Renci.SshNet;

namespace portal.Services;

public class DocumentService : IDocumentService
{
    private readonly ApplicationDbContext _ctx;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _storage;
    private readonly DocumentOptions _docOpts;
    private readonly ILogger<DocumentService> _logger;
    private readonly string _baseUrl;

    public DocumentService(
        ApplicationDbContext ctx,
        IMapper mapper,
        IFileStorageService storage,
        IOptions<DocumentOptions> docOpts,
        ILogger<DocumentService> logger
    )
    {
        _ctx = ctx;
        _mapper = mapper;
        _storage = storage;
        _docOpts = docOpts.Value;
        _baseUrl = _docOpts.PublicBaseUrl.TrimEnd('/');
        _logger = logger;
    }

    // Create a new document entry, not storing files
    public async Task<DocumentDTO> CreateMetaDataAsync(CreateDocumentDTO dto)
    {
        var entity = _mapper.Map<Document>(dto);
        _ctx.Documents.Add(entity);
        await _ctx.SaveChangesAsync();

        return _mapper.Map<DocumentDTO>(entity);
    }

    // Update the document metadata, but not the file itself
    public async Task<DocumentDTO> UpdateAsync(int id, UpdateDocumentDTO dto)
    {
        var entity =
            await _ctx.Documents.FindAsync(id)
            ?? throw new KeyNotFoundException($"Document {id} not found.");
        _mapper.Map(dto, entity);
        _ctx.Documents.Update(entity);
        await _ctx.SaveChangesAsync();
        return await GetByIdAsync(id)
            ?? throw new InvalidOperationException("Failed to retrieve document after update.");
    }

    // Delete the document and its associated file from storage
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _ctx.Documents.FindAsync(id);
        if (entity == null)
            return false;

        var category = entity.GeneralDocumentInfo.Category.ToString();
        var fileName = entity.GeneralDocumentInfo.Name;

        var remotePath = $"{_docOpts.StorageDir}/{category}/{fileName}";

        try
        {
            await _storage.DeleteAsync(remotePath);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to delete SFTP file {RemotePath} for Document {Id}",
                remotePath,
                id
            );
        }

        _ctx.Documents.Remove(entity);
        await _ctx.SaveChangesAsync();
        return true;
    }

    // Get the metadata and attach file into DTO
    public async Task<DocumentDTO?> GetByIdAsync(int id)
    {
        var entity = await _ctx.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

        if (entity is null)
            return null;

        var dto = _mapper.Map<DocumentDTO>(entity);

        var fileUrl = entity.GeneralDocumentInfo?.Url;
        if (!string.IsNullOrWhiteSpace(fileUrl))
        {
            try
            {
                var stream = await _storage.DownloadAsync(fileUrl);
                dto.File = new FormFile(
                    stream,
                    0,
                    stream.Length,
                    "file",
                    Path.GetFileName(fileUrl)
                );
            }
            catch (FileNotFoundException)
            {
                _logger.LogWarning("File not found for document {Id} at {Url}", id, fileUrl);
            }
        }

        return dto;
    }

    // Get all metadata without files
    public async Task<IEnumerable<DocumentDTO>> GetAllMetaDataAsync()
    {
        var entities = await _ctx.Documents.ToListAsync();
        return _mapper.Map<List<DocumentDTO>>(entities);
    }

    // Upload a new document file and create metadata entry
    public async Task<DocumentDTO> UploadDocumentAsync(CreateDocumentDTO dto)
    {
        var file = dto.File;
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is required.");

        var fileName = dto.GeneralDocumentInfo?.Name;
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Document name is required in GeneralDocumentInfo.Name");

        // Check for existing document name in DB
        var nameExists = await _ctx.Documents.AnyAsync(d => d.GeneralDocumentInfo.Name == fileName);
        if (nameExists)
            throw new InvalidOperationException(
                $"A document named '{fileName}' already exists in database."
            );

        // Determine category
        var category = dto.GeneralDocumentInfo?.Category ?? DocumentCategoryEnum.EQUIPMENT;

        // Construct remote path
        var remotePath = Path.Combine(_docOpts.StorageDir, category.ToString(), fileName)
            .Replace("\\", "/");

        // Check if file exists on SFTP
        var fileExists = await _storage.ExistsAsync(remotePath);
        if (fileExists)
            throw new InvalidOperationException(
                $"A file named '{fileName}' already exists on SFTP."
            );

        // Upload file
        using (var stream = file.OpenReadStream())
        {
            await _storage.UploadAsync(stream, remotePath);
        }

        // Set URL
        dto.GeneralDocumentInfo.Url = $"{_docOpts.PublicBaseUrl}/{category}/{fileName}";

        // Save metadata
        var entity = _mapper.Map<Document>(dto);
        _ctx.Documents.Add(entity);
        await _ctx.SaveChangesAsync();

        return _mapper.Map<DocumentDTO>(entity);
    }

    // Upload a new document file and replace existing metadata entry and the file
    public async Task<DocumentDTO> UploadAndReplaceDocumentAsync(UpdateDocumentDTO dto, int id)
    {
        if (dto.File == null)
            throw new ArgumentException("No file uploaded.");

        if (string.IsNullOrWhiteSpace(dto.GeneralDocumentInfo.Name))
            throw new ArgumentException("Document name is required.");

        if (dto.GeneralDocumentInfo.Category == null)
            throw new ArgumentException("Document category is required.");

        var category = dto.GeneralDocumentInfo.Category.ToString();
        var fileName = dto.GeneralDocumentInfo.Name;
        var relativePath = $"{category}/{fileName}";
        var fullPath = $"{_docOpts.StorageDir.TrimEnd('/')}/{relativePath}";

        if (await _storage.ExistsAsync(fullPath))
        {
            await _storage.DeleteAsync(fullPath);
        }

        await using var stream = dto.File.OpenReadStream();
        await _storage.UploadAsync(stream, fullPath);

        // Update the URL field in GeneralDocumentInfo
        dto.GeneralDocumentInfo.Url = $"{_docOpts.PublicBaseUrl.TrimEnd('/')}/{relativePath}";

        return await UpdateAsync(id, dto);
    }

    // Check if a file exists in the storage. Helper funcs
    public async Task<bool> IsFileExistAsync(string category, string fileName)
    {
        var remotePath = Path.Combine(_docOpts.StorageDir, category, fileName).Replace("\\", "/");
        return await _storage.ExistsAsync(remotePath);
    }
}
