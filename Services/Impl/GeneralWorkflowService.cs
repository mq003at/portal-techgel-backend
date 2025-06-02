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

        var dtos = _mapper.Map<List<GeneralWorkflowDTO>>(workflows);

        // For each workflow and each node, populate names
        foreach (var workflowDto in dtos)
        {
            foreach (var nodeDto in workflowDto.ApprovalWorkflowNodes)
            {
                await _nodeService.PopulateNamesAndDocsAsync(nodeDto);
            }
        }

        return dtos;
    }

    public override async Task<GeneralWorkflowDTO?> GetByIdAsync(int id)
    {
        var workflow = await _generalWorkflows
            .Include(g => g.ApprovalWorkflowNodes)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (workflow == null)
            return null;

        var dto = _mapper.Map<GeneralWorkflowDTO>(workflow);

        foreach (var nodeDto in dto.ApprovalWorkflowNodes)
        {
            await _nodeService.PopulateNamesAndDocsAsync(nodeDto);
        }

        return dto;
    }

    public async Task PopulateNamesAndDocsAsync(ApprovalWorkflowNodeDTO nodeDto)
    {
        // Populate SenderName
        var sender = await _context.Set<Employee>().FindAsync(nodeDto.SenderId);
        nodeDto.SenderName = sender != null
            ? $"{sender.FirstName} {sender.LastName}".Trim()
            : null;

        // Populate ReceiverNames
        if (nodeDto.ReceiverIds != null && nodeDto.ReceiverIds.Count > 0)
        {
            var receivers = await _context.Set<Employee>()
                .Where(e => nodeDto.ReceiverIds.Contains(e.Id))
                .ToListAsync();

            nodeDto.ReceiverNames = receivers
                .OrderBy(e => nodeDto.ReceiverIds.IndexOf(e.Id))
                .Select(e => $"{e.FirstName} {e.LastName}".Trim())
                .ToList();
        }

        // Populate DocumentNames and (optionally) URLs
        if (nodeDto.DocumentIds != null && nodeDto.DocumentIds.Count > 0)
        {
            var docIdList = nodeDto.DocumentIds.ToList();

            var documents = await _context.Set<Document>()
                .Where(d => docIdList.Contains(d.Id))
                .ToListAsync();

            nodeDto.DocumentNames = documents
                .OrderBy(d => docIdList.IndexOf(d.Id))
                .Select(d => d.GeneralDocumentInfo.Name)
                .ToList();

            _logger.LogInformation(
                "docId: {id}, docNames: {names}",
                nodeDto.DocumentIds.Count,
                nodeDto.DocumentNames.Count
            );
        }
    }


}
