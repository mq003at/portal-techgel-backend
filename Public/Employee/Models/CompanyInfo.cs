using portal.Enums;

namespace portal.Models;

public class CompanyInfo : BaseModelWithOnlyId
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
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
}

