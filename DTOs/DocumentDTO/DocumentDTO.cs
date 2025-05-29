using System.ComponentModel.DataAnnotations;
using portal.Models;

namespace portal.DTOs;

public class DocumentDTO : BaseDTO<Document>
{
    public IFormFile? File { get; set; } // For file upload scenarios
    public GeneralDocumentInfoDTO GeneralDocumentInfo { get; set; } = new();
    public LegalDocumentInfoDTO LegalDocumentInfo { get; set; } = new();
    public SecurityDocumentInfoDTO SecurityDocumentInfo { get; set; } = new();
    public AdditionalDocumentInfo AdditionalDocumentInfo { get; set; } = new();
    public List<EditDocumentInfoDTO> EditDocumentInfo { get; set; } = new();
}

public class CreateDocumentDTO : BaseDTO<Document>
{
    public IFormFile? File { get; set; } // For file upload scenarios

    [Required]
    public GeneralDocumentInfoDTO GeneralDocumentInfo { get; set; } = new();

    [Required]
    public LegalDocumentInfoDTO LegalDocumentInfo { get; set; } = new();
    public SecurityDocumentInfo SecurityDocumentInfo { get; set; } = new();
    public AdditionalDocumentInfo AdditionalDocumentInfo { get; set; } = new();
    public List<EditDocumentInfoDTO> EditDocumentInfoDTO { get; set; } = new();
}

public class UpdateDocumentDTO : BaseDTO<Document>
{
    public IFormFile? File { get; set; } // For file upload scenarios

    [Required]
    public GeneralDocumentInfoDTO GeneralDocumentInfo { get; set; } = new();

    [Required]
    public LegalDocumentInfoDTO LegalDocumentInfo { get; set; } = new();
    public SecurityDocumentInfo SecurityDocumentInfo { get; set; } = new();
    public AdditionalDocumentInfo AdditionalDocumentInfo { get; set; } = new();
    public List<EditDocumentInfoDTO> EditDocumentInfoDTO { get; set; } = new();
}

// DTO specifically for multipart file upload
public class UploadDocumentFileDTO
{
    [Required]
    public IFormFile File { get; set; } = null!;
}
