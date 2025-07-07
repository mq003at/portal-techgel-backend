using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IGeneralProposalNodeService
    : IBaseService<
        GeneralProposalNode,
        GeneralProposalNodeDTO,
        GeneralProposalNodeCreateDTO,
        GeneralProposalNodeUpdateDTO
    >
{
    Task<string> ApproveAsync(int nodeId, int approverId);
    Task<string> RejectAsync(int nodeId, int approverId, string rejectReason);
}
