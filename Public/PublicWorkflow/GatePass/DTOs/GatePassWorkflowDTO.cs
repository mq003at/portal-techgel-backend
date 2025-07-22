using System.ComponentModel.DataAnnotations;
using portal.Models;

namespace portal.DTOs;

public class GatePassWorkflowDTO : BaseWorkflowDTO
{
    public string Reason { get; set; } = null!;
    public DateTime GatePassStartTime { get; set; }
    public DateTime GatePassEndTime { get; set; }
    public string? Comment { get; set; }
    public string? RejectReason { get; set; }
    public List<GatePassNodeDTO> GatePassNodes { get; set; } = [];
}

public class GatePassWorkflowCreateDTO : BaseWorkflowCreateDTO
{
    [Required]
    public string Reason { get; set; } = null!;

    [Required]
    public DateTime GatePassStartTime { get; set; }

    [Required]
    public DateTime GatePassEndTime { get; set; }
}

public class GatePassWorkflowUpdateDTO : BaseWorkflowUpdateDTO
{
    public string? Reason { get; set; } = null!;
    public DateTime? GatePassStartTime { get; set; }
    public DateTime? GatePassEndTime { get; set; }
}
