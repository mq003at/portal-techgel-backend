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

    public override async Task<IEnumerable<GeneralWorkflowDTO>> GetAllAsync()
    {
        // Load all workflows with nodes in one query
        var workflows = await _generalWorkflows
            .Include(g => g.ApprovalWorkflowNodes)
            .ToListAsync();

        // Use the base.MapEntitiesToDTOs (if base expects entities) or base.Map (if available)
        // (Assume base.GetAllAsync fetches from context.Set<T> - which would not include nodes by default)
        // So: re-map here for performance
        return _mapper.Map<IEnumerable<GeneralWorkflowDTO>>(workflows);
    }

    public override async Task<GeneralWorkflowDTO?> GetByIdAsync(int id)
    {
        var workflow = await _generalWorkflows
            .Include(g => g.ApprovalWorkflowNodes)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (workflow == null)
            return null;

        return _mapper.Map<GeneralWorkflowDTO>(workflow);
    }

}
