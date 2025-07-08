namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface IGeneralProposalWorkflowService
    : IBaseWorkflowService<
        GeneralProposalWorkflow,
        GeneralProposalWorkflowDTO,
        GeneralProposalWorkflowCreateDTO,
        GeneralProposalWorkflowUpdateDTO,
        GeneralProposalNode
    >
{
    Task<List<GeneralProposalNodeDTO>> GenerateNodesAsync(
        GeneralProposalWorkflowCreateDTO dto,
        GeneralProposalWorkflow workflow
    );
    Task<IEnumerable<GeneralProposalNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId);

    Task<bool> FinalizeIfCompleteAsync(
        GeneralProposalWorkflow workflow,
        int approvalId,
        int nodeId
    );

    Task<bool> GenerateGeneralProposalFinalDocument(
        Employee employee,
        Employee approver,
        GeneralProposalWorkflow workflow,
        int nodeId
    );
    Task<List<GeneralProposalWorkflowDTO>> GetAllByEmployeeIdAsync(int id);
}
