using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IGeneralProposalNodeService
    : IBaseNodeService<
        GeneralProposalNode,
        GeneralProposalNodeDTO,
        GeneralProposalNodeCreateDTO,
        GeneralProposalNodeUpdateDTO,
        GeneralProposalWorkflow
    > { }
