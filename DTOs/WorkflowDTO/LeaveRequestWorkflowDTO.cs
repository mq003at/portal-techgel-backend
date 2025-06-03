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

    public ICollection<LeaveRequestNodeDTO> LeaveRequestNodes { get; set; } = new List<LeaveRequestNodeDTO>();
}

public class CreateLeaveRequestWorkflowDTO : CreateBaseWorkflowDTO<LeaveRequestWorkflow>
{
    public int EmployeeId { get; set; }
    public string Reason { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int EmployeeAnnualLeaveTotalDays { get; set; }


    // Optional: cho phép gửi kèm các bước, hoặc tạo tự động trong service
    public ICollection<CreateLeaveRequestNodeDTO> LeaveRequestNodes { get; set; } = new List<CreateLeaveRequestNodeDTO>();
}

public class UpdateLeaveRequestWorkflowDTO : UpdateBaseWorkflowDTO<LeaveRequestWorkflow>
{
    public int? EmployeeId { get; set; }
    public string? Reason { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ICollection<UpdateLeaveRequestNodeDTO>? LeaveRequestNodes { get; set; }
}