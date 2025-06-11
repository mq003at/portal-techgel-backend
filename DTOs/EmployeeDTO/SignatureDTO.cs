using System.ComponentModel.DataAnnotations;
using portal.DTOs.Validations;
using portal.Models;

namespace portal.DTOs;

public class SignatureDTO : BaseModelDTO<Signature>
{
    [Required]
    public int EmployeeId { get; set; }

    [Required, StringLength(255)]
    public string FileName { get; set; } = null!;

    [Required, Url]
    public string FileUrl { get; set; } = null!;
}

public class UploadSignatureDTO : BaseModelDTO<Signature>
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    [AllowedExtensions([".svg"], ErrorMessage = "Only SVG files are allowed.")]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = "File size cannot exceed 3 MB.")]
    public IFormFile File { get; set; } = null!;

    [
        Required,
        RegularExpression(
            @"^[a-zA-Z0-9_\-]+\.svg$",
            ErrorMessage = "Filename must be alphanumeric or _‐, ending in .svg"
        )
    ]
    public string FileName { get; set; } = null!;
}

public class UpdateSignatureDTO : BaseModelDTO<Signature>
{
    [AllowedExtensions([".svg"], ErrorMessage = "Only SVG files are allowed.")]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = "File size cannot exceed 3 MB.")]
    public IFormFile? File { get; set; }
}
