namespace portal.DTOs;

public class LeaveRequestWorkflowDTO : BaseWorkflowDTO
{
    public string Reason { get; set; } = string.Empty;
    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    public DateTime StartDate { get; set; }
    public DayNightEnum StartDateDayNightType { get; set; }
    public DateTime EndDate { get; set; }
    public DayNightEnum EndDateDayNightType { get; set; }

    public double TotalDays { get; set; }
    public double EmployeeAnnualLeaveTotalDays { get; set; }
    public double FinalEmployeeAnnualLeaveTotalDays { get; set; }
    public List<LeaveRequestNodeDTO> LeaveRequestNodes { get; set; } = new();
    public string? RejectReason { get; set; }
}

// Create form
public class LeaveRequestWorkflowCreateDTO : BaseWorkflowCreateDTO
{
    public required string Reason { get; set; } = null!;
    public required LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    public required DateTime StartDate { get; set; }
    public required DayNightEnum StartDateDayNightType { get; set; }
    public required DateTime EndDate { get; set; }
    public required DayNightEnum EndDateDayNightType { get; set; }
    public required int EmployeeId { get; set; }
    public required int AssigneeId { get; set; }
}

// Update Workflow (only use Comment from BaseWorkflowUpdateDTO)
public class LeaveRequestWorkflowUpdateDTO : BaseWorkflowUpdateDTO
{
    public string? Reason { get; set; }
}
