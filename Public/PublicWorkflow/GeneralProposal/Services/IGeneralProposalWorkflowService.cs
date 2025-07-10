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

    Task<bool> FinalizeIfCompleteAsync(int workflowId);

    Task<bool> GenerateGeneralProposalFinalDocument(
        Employee employee,
        Employee approver,
        GeneralProposalWorkflow workflow
    );
    Task<List<GeneralProposalWorkflowDTO>> GetAllByEmployeeIdAsync(int id);
}
