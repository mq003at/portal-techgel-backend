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
    Task<string> ApproveAsync(int nodeId, ApproveWithCommentDTO dto);
    Task<string> RejectAsync(int nodeId, RejectDTO dto);
}
