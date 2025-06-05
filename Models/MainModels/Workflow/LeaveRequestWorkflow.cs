using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class LeaveRequestWorkflow : BaseWorkflow
{
    [Required]
    public int EmployeeId { get; set; }
    [NotMapped]
    public Employee Employee { get; set; } = new();
    [NotMapped]
    public string EmployeeName { get; set; } = string.Empty;
    [Required]
    public string Reason { get; set; } = string.Empty;
    [Required]
    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    public int WorkAssignedToId { get; set; }
    [NotMapped]
    public Employee? WorkAssignedTo { get; set; } = null;
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DayNightEnum StartDateDayNightType { get; set; }
    public DateTime EndDate { get; set; }
    [Required]
    public DayNightEnum EndDateDayNightType { get; set; } = DayNightEnum.Day;
    public float TotalDays { get; set; } = 0f;
    public float EmployeeAnnualLeaveTotalDays { get; set; } = 0f;
    public float FinalEmployeeAnnualLeaveTotalDays { get; set; } = 0f;

    public List<LeaveRequestNode> LeaveRequestNodes { get; set; } = new List<LeaveRequestNode>();
}
