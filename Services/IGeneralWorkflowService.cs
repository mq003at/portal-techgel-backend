using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IGeneralWorkflowService
    : IBaseService<
        GeneralWorkflow,
        GeneralWorkflowDTO,
        CreateGeneralWorkflowDTO,
        UpdateGeneralWorkflowDTO
    >
{
    Task<IEnumerable<ApprovalWorkflowNodeDTO>> GetNodesAsync(int workflowId);
    Task<ApprovalWorkflowNodeDTO> AddNodeAsync(int workflowId, CreateApprovalWorkflowNodeDTO dto);
    Task RemoveNodeAsync(int workflowId, int nodeId);
}
