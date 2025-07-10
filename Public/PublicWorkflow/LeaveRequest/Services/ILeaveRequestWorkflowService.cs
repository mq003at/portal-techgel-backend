namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface ILeaveRequestWorkflowService
    : IBaseWorkflowService<
        LeaveRequestWorkflow,
        LeaveRequestWorkflowDTO,
        LeaveRequestWorkflowCreateDTO,
        LeaveRequestWorkflowUpdateDTO,
        LeaveRequestNode
    >
{
    Task<List<LeaveRequestNodeDTO>> GenerateNodesAsync(
        LeaveRequestWorkflowCreateDTO dto,
        LeaveRequestWorkflow workflow
    );
    Task<IEnumerable<LeaveRequestNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId);

    Task<bool> FinalizeIfCompleteAsync(int workflowId);

    Task<bool> GenerateLeaveRequestFinalDocument(
        Employee employee,
        Employee approver,
        LeaveRequestWorkflow workflow
    );
    Task<List<LeaveRequestWorkflowDTO>> GetAllByEmployeeIdAsync(int id);
}
