namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface ILeaveRequestWorkflowService : IBaseWorkflowService<
    LeaveRequestWorkflow,
    LeaveRequestWorkflowDTO,
    LeaveRequestWorkflowCreateDTO,
    LeaveRequestWorkflowUpdateDTO,
    LeaveRequestNode>
{
    Task<List<LeaveRequestNodeDTO>> GenerateNodesAsync(
        LeaveRequestWorkflowCreateDTO dto,
        LeaveRequestWorkflow workflow
    );

    Task<IEnumerable<LeaveRequestNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId);

    Task<bool> FinalizeIfCompleteAsync(int workflowId);

    Task<Document> GenerateLeaveRequestInitDocument(
        Employee employee,
        Employee assignee,
        Employee sender,
        Employee hr,
        Employee ceo,
        Employee supervisor,
        LeaveRequestWorkflowCreateDTO dto,
        double totalDays,
        double finalEmployeeAnnualLeaveTotalDays
    );
}
