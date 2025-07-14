using System.ComponentModel.DataAnnotations;

namespace portal.DTOs;

public class LeaveRequestWorkflowDTO : BaseWorkflowDTO
{
    public string Reason { get; set; } = string.Empty;
    public LeaveApprovalCategory LeaveApprovalCategory { get; set; }

    public DateTime StartDate { get; set; }
    public DayNightEnum StartDateDayNightType { get; set; }

    public DateTime EndDate { get; set; }
    public DayNightEnum EndDateDayNightType { get; set; }

    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = null!;
    public string EmployeeMainId { get; set; } = string.Empty;
    public double TotalDays { get; set; }
    public double EmployeeAnnualLeaveTotalDays { get; set; }
    public double FinalEmployeeAnnualLeaveTotalDays { get; set; }
    public double EmployeeCompensatoryLeaveTotalDays { get; set; }
    public double FinalEmployeeCompensatoryLeaveTotalDays { get; set; }

    public string? RejectReason { get; set; }
    public string? Comment { get; set; }
    public string? Notes { get; set; }
    public List<int> AssigneeDetails { get; set; } = new List<int>();
    public List<string>? AssigneeNames { get; set; } = new List<string>();
    public List<LeaveRequestNodeDTO> LeaveRequestNodes { get; set; } = new();
}

// Create form
public class LeaveRequestWorkflowCreateDTO : BaseWorkflowCreateDTO
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
    public DayNightEnum EndDateDayNightType { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    public string? Notes { get; set; }

    [Required]
    public List<int> AssigneeDetails { get; set; } = new List<int>();
}

// Update Workflow (only usable when workflow is in draft status)
public class LeaveRequestWorkflowUpdateDTO : BaseWorkflowUpdateDTO
{
    public string? Reason { get; set; }

    public string? Notes { get; set; }
    public DateTime? StartDate { get; set; }

    public DayNightEnum? StartDateDayNightType { get; set; }

    public DateTime? EndDate { get; set; }

    public DayNightEnum? EndDateDayNightType { get; set; }
    public int EmployeeId { get; set; }
    public List<int>? AssigneeDetails { get; set; } = new List<int>();
}
