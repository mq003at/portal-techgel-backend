using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class GeneralWorkflowService
    : BaseService<
        GeneralWorkflow,
        GeneralWorkflowDTO,
        CreateGeneralWorkflowDTO,
        UpdateGeneralWorkflowDTO
    >,
      IGeneralWorkflowService
{
    private readonly DbSet<GeneralWorkflow> _generalWorkflows;
    private readonly DbSet<ApprovalWorkflowNode> _nodes;
    private new readonly ApplicationDbContext _context;
    private readonly IApprovalWorkflowNodeService _nodeService;

    public GeneralWorkflowService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<GeneralWorkflowService> logger,
        IApprovalWorkflowNodeService nodeService
    )
        : base(context, mapper, logger)
    {
        _generalWorkflows = context.Set<GeneralWorkflow>();
        _nodes = context.Set<ApprovalWorkflowNode>();
        _nodeService = nodeService;
        _context = context;
    }
}
