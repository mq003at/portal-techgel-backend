using System.ComponentModel.DataAnnotations;

namespace portal.Models;

public class TaxInfo : BaseModelWithOnlyId
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    [Required]
    public string TaxCode { get; set; } = string.Empty; // Mã số thuế cá nhân

    public DateTime? RegistrationDate { get; set; }

    public int NumberOfDependents { get; set; } 

    public bool IsFamilyStatusRegistered { get; set; } = false;

    public string? Note { get; set; } // Ghi chú (e.g., MST chưa kích hoạt)
}
