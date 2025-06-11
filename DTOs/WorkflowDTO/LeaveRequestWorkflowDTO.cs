namespace portal.DTOs;

public class LeaveRequestWorkflowDTO : BaseWorkflowDTO
{
    public string Reason { get; set; } = string.Empty;
    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    public DateTime StartDate { get; set; }
    public DayNightEnum StartDateDayNightType { get; set; }
    public DateTime EndDate { get; set; }
    public DayNightEnum EndDateDayNightType { get; set; }

    public float TotalDays { get; set; }
    public float EmployeeAnnualLeaveTotalDays { get; set; }
    public float FinalEmployeeAnnualLeaveTotalDays { get; set; }

    public List<LeaveRequestNodeDTO> LeaveRequestNodes { get; set; } = new();
}

public class LeaveRequestWorkflowCreateDTO : BaseWorkflowCreateDTO
{
    public string Reason { get; set; } = string.Empty;
    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    public DateTime StartDate { get; set; }
    public DayNightEnum StartDateDayNightType { get; set; }
    public DateTime EndDate { get; set; }
    public DayNightEnum EndDateDayNightType { get; set; }

    public List<int> LeaveRequestNodeIds { get; set; } = new();
}

public class LeaveRequestWorkflowUpdateDTO : BaseWorkflowUpdateDTO
{
    public string Reason { get; set; } = string.Empty;
    public LeaveAprrovalCategory LeaveAprrovalCategory { get; set; }
    public DateTime StartDate { get; set; }
    public DayNightEnum StartDateDayNightType { get; set; }
    public DateTime EndDate { get; set; }
    public DayNightEnum EndDateDayNightType { get; set; }
    public List<int> LeaveRequestNodeIds { get; set; } = new();
}
