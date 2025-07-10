using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/general-proposal-nodes")]
public class GeneralProposalNodeController
    : BaseNodeController<
        GeneralProposalNode,
        GeneralProposalNodeDTO,
        GeneralProposalNodeCreateDTO,
        GeneralProposalNodeUpdateDTO,
        GeneralProposalWorkflow
    >
{
    public GeneralProposalNodeController(IGeneralProposalNodeService nodeService)
        : base(nodeService) { }
}
