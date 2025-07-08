namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface IGatePassWorkflowService
    : IBaseWorkflowService<
        GatePassWorkflow,
        GatePassWorkflowDTO,
        GatePassWorkflowCreateDTO,
        GatePassWorkflowUpdateDTO,
        GatePassNode
    >
{
    Task<List<GatePassNodeDTO>> GenerateNodesAsync(
        GatePassWorkflowCreateDTO dto,
        GatePassWorkflow workflow
    );
    Task<IEnumerable<GatePassNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId);

    Task<bool> FinalizeIfCompleteAsync(int workflowId);

    Task<bool> GenerateGatePassFinalDocument(
        Employee employee,
        Employee approver,
        GatePassWorkflow workflow,
        string approverPosition
    );
    Task<List<GatePassWorkflowDTO>> GetAllByEmployeeIdAsync(int id);
}
