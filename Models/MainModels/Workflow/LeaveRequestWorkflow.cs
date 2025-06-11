using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Models;

public class LeaveRequestWorkflow : BaseWorkflow
{
    [Required]
    public string Reason { get; set; } = string.Empty;
    [Required]
    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DayNightEnum StartDateDayNightType { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public DayNightEnum EndDateDayNightType { get; set; } = DayNightEnum.Day;

    // Meta data for the leave request
    public float TotalDays { get; set; } = 0f;
    public float EmployeeAnnualLeaveTotalDays { get; set; } = 0f;
    public float FinalEmployeeAnnualLeaveTotalDays { get; set; } = 0f;
    public virtual List<LeaveRequestNode> LeaveRequestNodes { get; set; } = new List<LeaveRequestNode>();
}
