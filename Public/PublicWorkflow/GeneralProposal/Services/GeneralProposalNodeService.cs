namespace portal.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portal.Db;
using portal.DTOs;
using portal.Enums;
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
        ILogger<
            BaseNodeService<
                GeneralProposalNode,
                GeneralProposalNodeDTO,
                GeneralProposalNodeCreateDTO,
                GeneralProposalNodeUpdateDTO,
                GeneralProposalWorkflow
            >
        > logger
    )
        : base(context, mapper, logger) { }
}
