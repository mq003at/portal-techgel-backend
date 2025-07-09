using System.ComponentModel.DataAnnotations;
using portal.Enums;
using portal.Models;

namespace portal.DTOs;

public class SignDocumentUploadDTO
{
    [Required]
    public IFormFile File { get; set; } = default!;
}

public class SignaturesInDocumentDTO
{
    public string EmployeeName { get; set; } = string.Empty;
    public string SignedAt { get; set; } = string.Empty;
}

public class DocumentDTO : BaseModelDTO
{
    public string Name { get; set; } = string.Empty;
    public DocumentCategoryEnum Category { get; set; }
    public DocumentStatusEnum Status { get; set; }
    public string FileExtension { get; set; } = string.Empty;
    public long SizeInBytes { get; set; }
    public string Division { get; set; } = null!;
    public string TemplateKey { get; set; } = string.Empty;
    public List<string> Tag { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public List<SignaturesInDocumentDTO> Signatures { get; set; } = new();
}

public class DocumentCreateDTO : BaseModelCreateDTO
{
    public IFormFile File { get; set; } = null!; // For file upload scenarios

    [Required]
    public DocumentCategoryEnum Category { get; set; }
    public List<string>? Tag { get; set; }

    public string Division { get; set; } = null!;

    public DocumentStatusEnum Status { get; set; } = DocumentStatusEnum.UNKNOWN;

    [Required]
    public string Description { get; set; } = null!;
}

public class DocumentTemplateCreateDTO : BaseModelCreateDTO
{
    [Required]
    public string TemplateKey { get; set; } = null!;

    [Required]
    public IFormFile File { get; set; } = null!; // For file upload scenarios

    [Required]
    public DocumentCategoryEnum Category { get; set; }

    [Required]
    public string Division { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}

// only allowing changing re-uploading file or channging tags

public class DocumentUpdateDTO : BaseModelUpdateDTO
{
    public IFormFile? File { get; set; } // For file upload scenarios
    public List<string>? Tag { get; set; }
    public DocumentStatusEnum? Status { get; set; }
    public string? TemplateKey { get; set; }
}

// DTO specifically for multipart file upload
public class UploadDocumentFileDTO
{
    [Required]
    public IFormFile File { get; set; } = null!;
    public DocumentStatusEnum? Status { get; set; }
}

public class FillInTemplateDTO
{
    [Required]
    public string TemplateKey { get; set; } = null!;

    [Required]
    public List<Dictionary<string, string>> Placeholders { get; set; } = new();

    // Optional: output format or document settings
    public string? OutputFileName { get; set; }
    public string? OutputFormat { get; set; } = "docx"; // or "pdf", etc.

    // Optional: return as stream, or save to URL
    public bool ReturnAsStream { get; set; } = false;
}

public class ReplaceDocumentPlaceholdersDTO
{
    [Required]
    public int DocumentId { get; set; }

    [Required]
    public List<Dictionary<string, string>> PlaceholderSets { get; set; } = new();

    // Optional: overwrite or create new
    public bool Overwrite { get; set; } = true;

    public string? OutputFileName { get; set; }
}
