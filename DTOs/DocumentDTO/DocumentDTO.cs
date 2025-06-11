using System.ComponentModel.DataAnnotations;
using portal.Models;

namespace portal.DTOs;

public class SignDocumentUploadDTO
{
    [Required]
    public IFormFile File { get; set; } = default!;
}

public class DocumentDTO : BaseModelDTO<Document>
{
    public IFormFile? File { get; set; } // For file upload scenarios
    public GeneralDocumentInfoDTO GeneralDocumentInfo { get; set; } = new();
    public LegalDocumentInfoDTO LegalDocumentInfo { get; set; } = new();
    public SecurityDocumentInfoDTO SecurityDocumentInfo { get; set; } = new();
    public AdditionalDocumentInfo AdditionalDocumentInfo { get; set; } = new();
    public List<EditDocumentInfoDTO> EditDocumentInfo { get; set; } = new();
}

public class CreateDocumentDTO : BaseModelDTO<Document>
{
    public IFormFile? File { get; set; } // For file upload scenarios

    [Required]
    public CreateGeneralDocumentInfoDTO GeneralDocumentInfo { get; set; } = new();

    [Required]
    public CreateLegalDocumentInfoDTO LegalDocumentInfo { get; set; } = new();
    public SecurityDocumentInfo SecurityDocumentInfo { get; set; } = new();
    public AdditionalDocumentInfo AdditionalDocumentInfo { get; set; } = new();
    public List<EditDocumentInfoDTO> EditDocumentInfo { get; set; } = new();
}

public class UpdateDocumentDTO : BaseModelDTO<Document>
{
    public IFormFile? File { get; set; } // For file upload scenarios

    [Required]
    public UpdateGeneralDocumentInfoDTO GeneralDocumentInfo { get; set; } = new();

    [Required]
    public UpdateLegalDocumentInfoDTO LegalDocumentInfo { get; set; } = new();
    public SecurityDocumentInfo SecurityDocumentInfo { get; set; } = new();
    public AdditionalDocumentInfo AdditionalDocumentInfo { get; set; } = new();
    public List<EditDocumentInfoDTO> EditDocumentInfo { get; set; } = new();
}

// DTO specifically for multipart file upload
public class UploadDocumentFileDTO
{
    [Required]
    public IFormFile File { get; set; } = null!;
}
