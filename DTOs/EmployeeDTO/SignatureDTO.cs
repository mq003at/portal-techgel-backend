using System.ComponentModel.DataAnnotations;
using portal.DTOs.Validations;
using portal.Models;

namespace portal.DTOs;

public class SignatureDTO : BaseModelDTO
{
    [Required]
    public int EmployeeId { get; set; }

    [Required, StringLength(255)]
    public string FileName { get; set; } = null!;

    [Required, Url]
    public string StoragePath { get; set; } = null!;
}

public class UploadSignatureDTO : BaseModelCreateDTO
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    [AllowedExtensions([".png"], ErrorMessage = "Only PNG files are allowed.")]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = "File size cannot exceed 3 MB.")]
    public IFormFile File { get; set; } = null!;

    [
        Required,
        RegularExpression(
            @"^[a-zA-Z0-9_\-]+\.png$",
            ErrorMessage = "Filename must be alphanumeric or _‐, ending in .png"
        )
    ]
    public string FileName { get; set; } = null!;
}

public class UpdateSignatureDTO : BaseModelUpdateDTO
{
    [AllowedExtensions([".png"], ErrorMessage = "Only PNG files are allowed.")]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = "File size cannot exceed 3 MB.")]
    public IFormFile? File { get; set; }
}
