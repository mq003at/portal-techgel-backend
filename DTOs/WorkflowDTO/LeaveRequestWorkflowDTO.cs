using System.ComponentModel.DataAnnotations;
using portal.Models;

namespace portal.DTOs;

public class LeaveRequestWorkflowDTO : BaseWorkflowDTO<LeaveRequestWorkflow>
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int TotalDays => (EndDate - StartDate).Days + 1;
    public float EmployeeAnnualLeaveTotalDays { get; set; } = 0f;
    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    public int WorkAssignedToId { get; set; }
    public string WorkAssignedToName = string.Empty;
    public string WorkAssignedToPosition = string.Empty;
    public string WorkAssignedToPhone = string.Empty;
    public string WorkAssignedToEmail = string.Empty;
    public string WorkAssignedToHomeAdress = string.Empty;

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
}

public class UpdateLeaveRequestWorkflowDTO : UpdateBaseWorkflowDTO<LeaveRequestWorkflow>
{
    public string? Reason { get; set; }

    public List<UpdateLeaveRequestNodeDTO>? LeaveRequestNodes { get; set; }
}