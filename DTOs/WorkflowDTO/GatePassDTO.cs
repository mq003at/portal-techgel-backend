using portal.Models;

namespace portal.DTOs;

public class GatePassDTO : BaseWorkflowDTO<GatePass>
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // public List<GatePassNodeDTO> GatePassNodes { get; set; } = new List<GatePassNodeDTO>();
}