using System.ComponentModel.DataAnnotations;

namespace portal.Models;

public class DocumentSignature : BaseModelWithOnlyId
{
    [Required]
    public int DocumentId { get; set; }
    public Document Document { get; set; } = null!;

    [Required]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public DateTime SignedAt { get; set; }
    public string SignatureUrl { get; set; } = string.Empty; // Optional
}