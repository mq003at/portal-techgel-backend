using System.ComponentModel.DataAnnotations;
using portal.Models;
using portal.Services;

namespace portal.DTOs;

public class LeaveRequestWorkflowDTO : BaseWorkflowDTO<LeaveRequestWorkflow>
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeMainId { get; set; } = string.Empty;
    public DayNightEnum StartDateDayNightType { get; set; }
    public DayNightEnum EndDateDayNightType { get; set; }
    public string Reason { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public float TotalDays { get; set; }
    public float EmployeeAnnualLeaveTotalDays { get; set; }
    public float FinalEmployeeAnnualLeaveTotalDays { get; set; }
    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    public int WorkAssignedToId { get; set; }
    public string? WorkAssignedToName { get; set; }
    public string? WorkAssignedToPosition { get; set; }
    public string? WorkAssignedToPhone { get; set; }
    public string? WorkAssignedToEmail { get; set; }
    public string? WorkAssignedToHomeAdress { get; set; }
    public List<Attachment>? Attachments { get; set; }

    public List<LeaveRequestNodeDTO> LeaveRequestNodes { get; set; } = new List<LeaveRequestNodeDTO>();
}

public class CreateLeaveRequestWorkflowDTO : CreateBaseWorkflowDTO<LeaveRequestWorkflow>
{
    [Required]
    public required int EmployeeId { get; set; }
    [Required]

    public required string Reason { get; set; }
    [Required]

    public DateTime StartDate { get; set; }
    [Required]

    public DayNightEnum StartDateDayNightType { get; set; }
    [Required]

    public DateTime EndDate { get; set; }
    [Required]

    public DayNightEnum EndDateDayNightType { get; set; }
    [Required]

    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    [Required]

    public required int WorkAssignedToId { get; set; }

    public float? FinalEmployeeAnnualLeaveTotalDays { get; set; }
    public float? EmployeeAnnualLeaveTotalDays { get; set; }

}

public class UpdateLeaveRequestWorkflowDTO : UpdateBaseWorkflowDTO<LeaveRequestWorkflow>
{
    public string? Reason { get; set; }

    public List<UpdateLeaveRequestNodeDTO>? LeaveRequestNodes { get; set; }
}