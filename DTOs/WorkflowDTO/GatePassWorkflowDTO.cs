using System.ComponentModel.DataAnnotations;
using portal.Models;

namespace portal.DTOs;

public class GatePassWorkflowDTO : BaseWorkflowDTO
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = null!;
    public string Reason { get; set; } = null!;
    public DateTime GatePassStartTime { get; set; }
    public DateTime GatePassEndTime { get; set; }
}

public class CreateGatePassWorkflowDTO : BaseWorkflowCreateDTO
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public string Reason { get; set; } = null!;

    [Required]
    public DateTime GatePassStartTime { get; set; }

    [Required]
    public DateTime GatePassEndTime { get; set; }
}

public class UpdateGatePassWorkflowDTO : BaseWorkflowUpdateDTO
{
    [Required]
    public string Reason { get; set; } = null!;
}