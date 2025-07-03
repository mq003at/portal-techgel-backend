using System.ComponentModel.DataAnnotations;

namespace portal.DTOs;

public class TaxInfoDTO : BaseModelWithOnlyIdDTO
{
    public int EmployeeId { get; set; }
    public string TaxCode { get; set; } = string.Empty;
    public DateTime? RegistrationDate { get; set; }
    public int NumberOfDependents { get; set; }
    public bool IsFamilyStatusRegistered { get; set; }
    public string? Note { get; set; }
}

public class CreateTaxInfoDTO : BaseModelWithOnlyIdCreateDTO
{
    public int EmployeeId { get; set; }

    [Required]
    public string TaxCode { get; set; } = string.Empty;

    public DateTime? RegistrationDate { get; set; }
    public int NumberOfDependents { get; set; }
    public bool IsFamilyStatusRegistered { get; set; } = false;
    public string? Note { get; set; }
}

public class UpdateTaxInfoDTO : BaseModelWithOnlyIdUpdateDTO
{
    public string? TaxCode { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public int? NumberOfDependents { get; set; }
    public bool? IsFamilyStatusRegistered { get; set; }
    public string? Note { get; set; }
}
