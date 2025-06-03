namespace portal.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;


public class LeaveRequestWorkflowService : BaseService<
    LeaveRequestWorkflow,
    LeaveRequestWorkflowDTO,
    CreateLeaveRequestWorkflowDTO,
    UpdateLeaveRequestWorkflowDTO>,
    ILeaveRequestWorkflowService
{
    private new readonly IMapper _mapper;
    private new readonly ApplicationDbContext _context;
    private new readonly ILogger<LeaveRequestWorkflowService> _logger;

    public LeaveRequestWorkflowService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<LeaveRequestWorkflowService> logger
    ) : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public Task<List<LeaveRequestNodeDTO>> GenerateStepsAsync(CreateLeaveRequestWorkflowDTO dto)
    {
        var totalDays = (dto.EndDate - dto.StartDate).Days + 1;

        var steps = new List<LeaveRequestNode>
        {
            new() { StepType = LeaveApprovalStepType.ManagerApproval, SenderId = dto.DraftedByIds.FirstOrDefault() },
            new() { StepType = LeaveApprovalStepType.CBApproval, SenderId = 0 },
            new() { StepType = LeaveApprovalStepType.SummaryTracking, SenderId = 0 }
        };

        if (totalDays >= 3)
        {
            steps.Add(new() { StepType = LeaveApprovalStepType.HRHeadApproval, SenderId = 0 });
            steps.Add(new() { StepType = LeaveApprovalStepType.ExecutiveApproval, SenderId = 0 });
        }

        steps.Add(new() { StepType = LeaveApprovalStepType.FinalizeToPayroll, SenderId = 0 });

        return Task.FromResult(_mapper.Map<List<LeaveRequestNodeDTO>>(steps));
    }

    public async Task<bool> ApproveNodeAsync(int nodeId, int approverId, string? comment = null)
    {
        var node = await _context.LeaveRequestNodes.FindAsync(nodeId);
        if (node == null) return false;

        if (!node.HasBeenApprovedByIds.Contains(approverId))
        {
            node.HasBeenApprovedByIds.Add(approverId);
            node.ApprovedDates.Add(DateTime.UtcNow);
        }

        _context.LeaveRequestNodes.Update(node);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<LeaveRequestNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId)
    {
        var nodes = await _context.LeaveRequestNodes
            .Where(n => n.LeaveRequestWorkflowId == workflowId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<LeaveRequestNodeDTO>>(nodes);
    }

    public async Task<bool> FinalizeIfCompleteAsync(int workflowId)
    {
        var workflow = await _context.LeaveRequestWorkflows
            .Include(w => w.LeaveRequestNodes)
            .FirstOrDefaultAsync(w => w.Id == workflowId);

        if (workflow == null) return false;

        var allApproved = workflow.LeaveRequestNodes.All(n =>
            n.Status == GeneralWorkflowStatusType.Approved);

        if (allApproved)
        {
            workflow.Status = GeneralWorkflowStatusType.Approved;
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
