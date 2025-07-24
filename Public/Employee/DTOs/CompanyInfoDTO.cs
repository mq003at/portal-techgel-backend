using portal.Enums;

namespace portal.DTOs;

public class CompanyInfoDTO : BaseModelDTO
{
    public string? CompanyEmail { get; set; }
    public string? CompanyPhoneNumber { get; set; }
    public EmploymentStatus EmploymentStatus { get; set; }
    public string? Position { get; set; }
    public string? Department { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ProbationStartDate { get; set; }
    public DateTime? ProbationEndDate { get; set; }
    public bool IsOnProbation { get; set; }
    public double CompensatoryLeaveTotalDays { get; set; }
    public double AnnualLeaveTotalDays { get; set; }
    public double? AdditionalLeaveForPosition { get; set; }
}

public class CompanyInfoUpdateDTO : BaseModelUpdateDTO
{
    public string? CompanyEmail { get; set; }
    public string? CompanyPhoneNumber { get; set; }
    public EmploymentStatus? EmploymentStatus { get; set; }
    public string? Position { get; set; }
    public string? Department { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ProbationStartDate { get; set; }
    public DateTime? ProbationEndDate { get; set; }
    public bool? IsOnProbation { get; set; }
    public double? CompensatoryLeaveTotalDays { get; set; }
    public double? AnnualLeaveTotalDays { get; set; }
    public double? AdditionalLeaveForPosition { get; set; }
}

public class CompanyInfoCreateDTO : BaseModelCreateDTO
{
    public string? CompanyEmail { get; set; }
    public string? CompanyPhoneNumber { get; set; }
    public EmploymentStatus EmploymentStatus { get; set; }
    public string? Position { get; set; }
    public string? Department { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ProbationStartDate { get; set; }
    public DateTime? ProbationEndDate { get; set; }
    public bool IsOnProbation { get; set; }
    public double CompensatoryLeaveTotalDays { get; set; }
    public double AnnualLeaveTotalDays { get; set; }
    public double? AdditionalLeaveForPosition { get; set; }
}
