using System.ComponentModel.DataAnnotations;
using portal.Models;

namespace portal.DTOs;

public class GatePassWorkflowDTO : BaseWorkflowDTO
{
    public string Reason { get; set; } = null!;
    public DateTimeOffset GatePassStartTime { get; set; }
    public DateTimeOffset GatePassEndTime { get; set; }
    public string? Comment { get; set; }
    public string? RejectReason { get; set; }
    public List<GatePassNodeDTO> GatePassNodes { get; set; } = [];
}

public class GatePassWorkflowCreateDTO : BaseWorkflowCreateDTO
{
    [Required]
    public string Reason { get; set; } = null!;

    [Required]
    public DateTimeOffset GatePassStartTime { get; set; }

    [Required]
    public DateTimeOffset GatePassEndTime { get; set; }
}

public class GatePassWorkflowUpdateDTO : BaseWorkflowUpdateDTO
{
    public string? Reason { get; set; } = null!;
    public DateTimeOffset? GatePassStartTime { get; set; }
    public DateTimeOffset? GatePassEndTime { get; set; }
}
