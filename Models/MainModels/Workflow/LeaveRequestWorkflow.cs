using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class LeaveRequestWorkflow : BaseWorkflow
{
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public string Reason { get; set; } = string.Empty;
    [Required]
    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    public int WorkAssignedToId { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DayNightEnum StartDateDayNightType { get; set; }
    public DateTime EndDate { get; set; }
    [Required]
    public DayNightEnum EndDateDayNightType { get; set; } = DayNightEnum.Day;
    [Required]
    public float TotalDays { get; set; } = 0f;
    [Required]
    public float EmployeeAnnualLeaveTotalDays { get; set; } = 0f;
    [Required]
    public float FinalEmployeeAnnualLeaveTotalDays { get; set; } = 0f;
    [Required]
    public List<LeaveRequestNode> LeaveRequestNodes { get; set; } = new List<LeaveRequestNode>();
}
