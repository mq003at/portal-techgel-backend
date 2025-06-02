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
    }

    // Get all nodes for a workflow
    public async Task<IEnumerable<ApprovalWorkflowNodeDTO>> GetNodesAsync(int workflowId)
    {
        var workflow = await _generalWorkflows
            .Include(gw => gw.ApprovalWorkflowNodes)
            .FirstOrDefaultAsync(gw => gw.Id == workflowId);

        if (workflow == null)
            throw new KeyNotFoundException($"GeneralWorkflow {workflowId} not found.");

        // Assuming ApprovalWorkflowNodes is a collection of ApprovalWorkflowNode entities
        var nodeIds = workflow.ApprovalWorkflowNodes.Select(n => n.Id).ToList();
        var nodes = await _nodes
            .Where(node => nodeIds.Contains(node.Id))
            .ToListAsync();

        return _mapper.Map<IEnumerable<ApprovalWorkflowNodeDTO>>(nodes);
    }

    // Add a node to a workflow
    public async Task<ApprovalWorkflowNodeDTO> AddNodeAsync(int workflowId, CreateApprovalWorkflowNodeDTO dto)
    {
        var workflow = await _generalWorkflows.FindAsync(workflowId);
        if (workflow == null)
            throw new KeyNotFoundException($"GeneralWorkflow {workflowId} not found.");

        // Use node service to create node
        var nodeDto = await _nodeService.CreateAsync(dto);

        // Retrieve the created node entity
        var nodeEntity = await _nodes.FindAsync(nodeDto.Id);
        if (nodeEntity == null)
            throw new KeyNotFoundException($"ApprovalWorkflowNode {nodeDto.Id} not found.");

        // Add node entity to workflow
        workflow.ApprovalWorkflowNodes.Add(nodeEntity);
        _generalWorkflows.Update(workflow);
        await _context.SaveChangesAsync();

        return nodeDto;
    }

    // Remove a node from a workflow
    public async Task RemoveNodeAsync(int workflowId, int nodeId)
    {
        var workflow = await _generalWorkflows.FindAsync(workflowId);
        if (workflow == null)
            throw new KeyNotFoundException($"GeneralWorkflow {workflowId} not found.");

        // Remove node entity from workflow
        var nodeEntity = workflow.ApprovalWorkflowNodes.FirstOrDefault(n => n.Id == nodeId);
        if (nodeEntity == null)
            throw new KeyNotFoundException($"Node {nodeId} is not part of workflow {workflowId}.");

        workflow.ApprovalWorkflowNodes.Remove(nodeEntity);
        _generalWorkflows.Update(workflow);
        await _context.SaveChangesAsync();

        // Optionally: use nodeService to delete the node entity
        // await _nodeService.DeleteAsync(nodeId);
    }
}
