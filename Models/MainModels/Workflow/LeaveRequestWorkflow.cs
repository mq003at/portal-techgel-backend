using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class LeaveRequestWorkflow : BaseWorkflow
{
    [Required]
    public string Reason { get; set; } = string.Empty;
    [Required]
    public LeaveApprovalCategory LeaveApprovalCategory { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DayNightEnum StartDateDayNightType { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public DayNightEnum EndDateDayNightType { get; set; } = DayNightEnum.Day;

    // Metadata for the leave request
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public double TotalDays { get; set; }
    public double EmployeeAnnualLeaveTotalDays { get; set; }
    public double FinalEmployeeAnnualLeaveTotalDays { get; set; }
    public double EmployeeCompensatoryLeaveTotalDays { get; set; }
    public double FinalEmployeeCompensatoryLeaveTotalDays { get; set; }
    public virtual List<LeaveRequestNode> LeaveRequestNodes { get; set; } = new List<LeaveRequestNode>();
    public string? Notes { get; set; }
    public string AssigneeDetails { get; set; } = null!;
}
