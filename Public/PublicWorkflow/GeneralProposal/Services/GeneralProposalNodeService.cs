namespace portal.Services;

using AutoMapper;
using Microsoft.Extensions.Logging;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class GeneralProposalNodeService
    : BaseNodeService<
        GeneralProposalNode,
        GeneralProposalNodeDTO,
        GeneralProposalNodeCreateDTO,
        GeneralProposalNodeUpdateDTO,
        GeneralProposalWorkflow
    >,
        IGeneralProposalNodeService
{
    public GeneralProposalNodeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<GeneralProposalNodeService> logger
    )
        : base(context, mapper, logger) { }
}
