using portal.Models;

namespace portal.DTOs;

public class InsuranceInfoDTO : BaseModelWithOnlyId
{
    public int EmployeeId { get; set; }

    public string InsuranceCode { get; set; } = null!;

    public string RegistrationLocation { get; set; } = null!;

    public DateTime EffectiveDate { get; set; }

    public DateTime? TerminationDate { get; set; }

    public string InsuranceStatus { get; set; } = null!;

    public string? Note { get; set; }
}

public class InsuranceInfoCreateDTO
{
    public int EmployeeId { get; set; }
    public string InsuranceCode { get; set; } = null!;
    public string RegistrationLocation { get; set; } = null!;
    public DateTime EffectiveDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public string InsuranceStatus { get; set; } = null!;
    public string? Note { get; set; }
}

public class InsuranceInfoUpdateDTO
{
    public string? InsuranceCode { get; set; } = null!;
    public string? RegistrationLocation { get; set; } = null!;
    public DateTime? EffectiveDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public string? InsuranceStatus { get; set; } = null!;
    public string? Note { get; set; }
}
