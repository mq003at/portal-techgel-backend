using portal.Enums;

namespace portal.DTOs;

public class CompanyInfoDTO
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
    public float? AnnualLeaveTotalDays { get; set; } = 0f;
}
