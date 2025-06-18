using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;
using portal.Options;
using Renci.SshNet;
using portal.Helpers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace portal.Services;

public class DocumentService : IDocumentService
{
    private readonly ApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IMapper _mapper;
    private readonly string _basePath;

    public DocumentService(ApplicationDbContext context, IFileStorageService fileStorage, IMapper mapper)
    {
        _context = context;
        _fileStorage = fileStorage;
        _mapper = mapper;
        _basePath = AppDomain.CurrentDomain.BaseDirectory; // Or set this to your desired base path
    }

    // --------------------------- METADATA ---------------------------

    public async Task<DocumentDTO> CreateMetaDataAsync(DocumentCreateDTO dto)
    {
        var entity = _mapper.Map<Models.Document>(dto);
        _context.Documents.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<DocumentDTO>(entity);
    }

    public async Task<DocumentDTO> UpdateMetaDataAsync(int id, DocumentUpdateDTO dto)
    {
        var doc = await _context.Documents.FindAsync(id);
        if (doc == null) throw new KeyNotFoundException("Document not found");

        _mapper.Map(dto, doc);
        await _context.SaveChangesAsync();
        return _mapper.Map<DocumentDTO>(doc);
    }

    public async Task<DocumentDTO?> GetMetaDataByIdAsync(int id)
    {
        var doc = await _context.Documents.Include(d => d.Signatures).FirstOrDefaultAsync(d => d.Id == id);
        return doc == null ? null : _mapper.Map<DocumentDTO>(doc);
    }

    public async Task<IEnumerable<DocumentDTO>> GetAllMetaDataAsync()
    {
        var docs = await _context.Documents.Include(d => d.Signatures).ToListAsync();
        return _mapper.Map<List<DocumentDTO>>(docs);
    }

    public async Task<bool> DeleteMetaDataAsync(int id)
    {
        var doc = await _context.Documents.FindAsync(id);
        if (doc == null) return false;
        _context.Documents.Remove(doc);
        await _context.SaveChangesAsync();
        return true;
    }

    // --------------------------- FILES ---------------------------

    public async Task<DocumentDTO> UploadDocumentAsync(DocumentCreateDTO dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            throw new ArgumentException("File is required");

        var fileName = dto.File.FileName;
        var fileStream = dto.File.OpenReadStream();

        var savedPath = await _fileStorage.UploadAsync(fileStream, fileName);

        var entity = _mapper.Map<Models.Document>(dto);
        entity.Url = savedPath;
        entity.FileExtension = Path.GetExtension(fileName);
        entity.SizeInBytes = dto.File.Length;

        _context.Documents.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<DocumentDTO>(entity);
    }

    public async Task<DocumentDTO> UploadAndReplaceDocumentAsync(DocumentUpdateDTO dto, int id)
    {
        var doc = await _context.Documents.FindAsync(id);
        if (doc == null) throw new KeyNotFoundException("Document not found");

        if (dto.File == null || dto.File.Length == 0)
            throw new ArgumentException("File is required");

        var fileName = dto.File.FileName;
        var fileStream = dto.File.OpenReadStream();

        await _fileStorage.ReplaceFileAsync(doc.Url, fileStream);
        doc.FileExtension = Path.GetExtension(fileName);
        doc.SizeInBytes = dto.File.Length;
        doc.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return _mapper.Map<DocumentDTO>(doc);
    }

    public async Task<bool> IsFileExistAsync(string category, string fileName)
    {
        var path = Path.Combine(category, fileName).Replace("\\", "/");
        return await _fileStorage.Exists(path);
    }

    public async Task<bool> IsUrlAccessibleAsync(string url)
    {
        return await _fileStorage.Exists(url);
    }

    public async Task<DocumentStatusEnum> CheckDocumentSignStatusAsync(int id)
    {
        var doc = await _context.Documents.Include(d => d.Signatures).FirstOrDefaultAsync(d => d.Id == id);
        return doc?.Signatures?.Any() == true ? DocumentStatusEnum.APPROVED : DocumentStatusEnum.REJECTED;
    }

    public async Task<bool> SignFileAsync(int documentId, Stream signedFileStream)
    {
        var doc = await _context.Documents.FindAsync(documentId);
        if (doc == null) return false;

        await _fileStorage.ReplaceFileAsync(doc.Url, signedFileStream);
        doc.Status = DocumentStatusEnum.APPROVED;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<DocumentDTO>> SearchByTagsAsync(List<string> tags)
    {
        var docs = await _context.Documents
            .Where(d => d.Tag.Any(t => tags.Contains(t)))
            .ToListAsync();
        return _mapper.Map<List<DocumentDTO>>(docs);
    }

    // --------------------------- TEMPLATES ---------------------------

    public async Task<DocumentDTO> CreateTemplateAsync(DocumentTemplateCreateDTO dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            throw new ArgumentException("File is required");
            
        var today = DateTime.UtcNow;

        // Generate filename + path
        var documentPath = Path.Combine("erp", "documents").Replace("\\", "/");
        var fileName = $"{today:yyyy-MM-dd}-{dto.Division}-BM-v01{Path.GetExtension(dto.File.FileName)}";
        var directoryPath = Path.Combine(documentPath,dto.Division, "Bieu_Mau").Replace("\\", "/");
        var targetPath = Path.Combine(directoryPath, fileName).Replace("\\", "/");
        var fullPath = Path.Combine(_basePath, targetPath);

        using var stream = dto.File.OpenReadStream();
        await _fileStorage.UploadAsync(stream, targetPath);

        var document = _mapper.Map<Models.Document>(dto);
        document.Name = fileName;
        document.Url = targetPath;
        document.FileExtension = Path.GetExtension(fileName);
        document.SizeInBytes = dto.File.Length;

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return _mapper.Map<DocumentDTO>(document);
    }
    public async Task<DocumentDTO> GetTemplateAsync(string templateKey)
    {
        var doc = await _context.Documents.FirstOrDefaultAsync(d => d.TemplateKey == templateKey);
        if (doc == null) throw new KeyNotFoundException("Template not found");
        return _mapper.Map<DocumentDTO>(doc);
    }

    public async Task<DocumentDTO> UpdateTemplateAsync(DocumentUpdateDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TemplateKey))
            throw new ArgumentException("Template key required");

        var doc = await _context.Documents.FirstOrDefaultAsync(d => d.TemplateKey == dto.TemplateKey);
        if (doc == null) throw new KeyNotFoundException("Template not found");

        _mapper.Map(dto, doc);
        await _context.SaveChangesAsync();
        return _mapper.Map<DocumentDTO>(doc);
    }


    private async Task<MemoryStream> FillPlaceholdersAsync(Stream templateStream, Dictionary<string, string> placeholders)
    {
        var output = new MemoryStream();
        await templateStream.CopyToAsync(output);
        output.Position = 0;
        return output;
    }
    
    public async Task<DocumentDTO> FillInTemplateAsync(FillInTemplateDTO dto)
    {
        var templateDoc = await _context.Documents.FirstOrDefaultAsync(d => d.TemplateKey == dto.TemplateKey);
        if (templateDoc == null) throw new KeyNotFoundException("Template not found");

        var filledFileName = dto.OutputFileName ?? ($"{Guid.NewGuid() + dto.TemplateKey}.docx");
        var filledFilePath = await ReplaceTextInFileAsync(new ReplaceDocumentPlaceholdersDTO
        {
            DocumentId = templateDoc.Id,
            PlaceholderSets = dto.Placeholders,
            OutputFileName = filledFileName,
            Overwrite = false
        });

        var newDoc = new Models.Document
        {
            Name = filledFileName,
            Url = filledFilePath,
            TemplateKey = templateDoc.TemplateKey,
            Description = "Filled from template",
            Tag = templateDoc.Tag,
            Category = templateDoc.Category,
            FileExtension = ".docx",
            SizeInBytes = new FileInfo(Path.Combine(_basePath, filledFilePath)).Length,
            Status = DocumentStatusEnum.DRAFT,
            Version = "1.0"
        };

        _context.Documents.Add(newDoc);
        await _context.SaveChangesAsync();
        return _mapper.Map<DocumentDTO>(newDoc);
    }

    public async Task<string> ReplaceTextInFileAsync(ReplaceDocumentPlaceholdersDTO dto)
    {
        var doc = await _context.Documents.FindAsync(dto.DocumentId);
        if (doc == null) throw new KeyNotFoundException("Document not found");

        var originalPath = Path.Combine(_basePath, doc.Url);
        var targetFileName = dto.Overwrite ? doc.Url : dto.OutputFileName ?? ($"filled_{Guid.NewGuid()}.docx");
        var targetPath = Path.Combine(_basePath, targetFileName);

        if (!dto.Overwrite)
        {
            File.Copy(originalPath, targetPath, true);
        }

        using var wordDoc = WordprocessingDocument.Open(targetPath, true);
        var body = wordDoc.MainDocumentPart?.Document?.Body;
        if (body == null)
            throw new InvalidDataException("Invalid Word document");

        foreach (var text in body.Descendants<Text>())
        {
            if (text.Text.Contains("{{"))
            {
                foreach (var placeholders in dto.PlaceholderSets)
                {
                    foreach (var kvp in placeholders)
                    {
                        text.Text = text.Text.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);
                    }
                }
            }
        }

        if (wordDoc.MainDocumentPart?.Document != null)
        {
            wordDoc.MainDocumentPart.Document.Save();
        }
        else
        {
            throw new InvalidDataException("Invalid Word document: Document part is missing.");
        }
        return dto.Overwrite ? doc.Url : targetFileName;
    }

}