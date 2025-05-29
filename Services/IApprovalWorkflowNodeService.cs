using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IApprovalWorkflowNodeService
{
    Task<ApprovalWorkflowNodeFileResultDTO> UpdateFilesFromNodeAsync(
        UpdateFilesInApprovalWorkflowNodesDTO dto
    );
    Task DeleteFilesFromNodeAsync(DeleteFilesFromApprovalWorkflowNodesDTO dto);
}
