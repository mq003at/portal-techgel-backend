namespace portal.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;
using Serilog;

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

    public async Task<List<LeaveRequestNodeDTO>> GenerateStepsAsync(
        CreateLeaveRequestWorkflowDTO dto,
        LeaveRequestWorkflow workflow)
    {
        // Get the last node ID (handle empty table)
        var lastId = await _context.LeaveRequestNodes.AnyAsync()
            ? await _context.LeaveRequestNodes.MaxAsync(x => x.Id)
            : 0;

        // Calculate number of leave days (min 0.5)
        var totalDays = (dto.EndDate - dto.StartDate).Days +
                        (dto.EndDateDayNightType - dto.StartDateDayNightType) * 0.5;
        if (totalDays < 0.4)
            throw new ArgumentException("End date must be after start date and at least half a day apart.");

        // Fetch annual leave and update DTO
        var employee = await _context.Employees
            .Where(e => e.Id == dto.EmployeeId)
            .Select(e => new { e.CompanyInfo.AnnualLeaveTotalDays, e.RoleInfo.SupervisorId })
            .FirstOrDefaultAsync();

        if (employee == null)
            throw new InvalidOperationException($"Employee {dto.EmployeeId} not found.");

        dto.EmployeeAnnualLeaveTotalDays = employee.AnnualLeaveTotalDays;
        dto.FinalEmployeeAnnualLeaveTotalDays = employee.AnnualLeaveTotalDays - (float)totalDays;

        // Get Director (Executive) and HR Head
        var directorId = await _context.OrganizationEntities
            .Where(e => e.Id == 3)
            .Select(e => e.ManagerId)
            .FirstOrDefaultAsync();

        if (employee.SupervisorId is null || directorId is null)
            throw new InvalidOperationException("Workflow is missing required manager IDs.");

        // Build up receiver IDs for workflow
        workflow.ReceiverIds.AddRange(new[] { dto.SenderId, employee.SupervisorId.Value, directorId.Value });

        // Compose workflow steps
        var steps = new List<(string Name, LeaveApprovalStepType StepType, int Status, List<int> Approvers, List<int> Approved)>
    {
        ("Tạo yêu cầu nghỉ phép", LeaveApprovalStepType.CreateForm, 1, new() { dto.SenderId }, new() { dto.SenderId }),
        ("Quản lý trực thuộc ký", LeaveApprovalStepType.ManagerApproval, 0, new() { employee.SupervisorId.Value }, new())
    };

        if (totalDays >= 3.0)
        {
            var hrHeadId = await _context.OrganizationEntities
                .Where(e => e.Id == 11)
                .Select(e => e.ManagerId)
                .FirstOrDefaultAsync();

            if (hrHeadId is null)
                throw new InvalidOperationException("HR Head does not have a valid manager.");

            steps.Add(("Trưởng phòng nhân sự ký", LeaveApprovalStepType.HRHeadApproval, 2, new() { hrHeadId.Value }, new()));
            workflow.ReceiverIds.Add(hrHeadId.Value);
        }

        steps.Add(("Ký xác nhận cuối cùng", LeaveApprovalStepType.ExecutiveApproval, 2, new() { directorId.Value }, new()));

        // Build and add nodes
        var nodes = new List<LeaveRequestNode>();
        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];
            var newId = lastId + i + 1;

            var node = new LeaveRequestNode
            {
                LeaveRequestWorkflowId = workflow.Id,
                StepType = step.StepType,
                MainId = $"AS-S{i}-{DateTime.Now:dd.MM.yyyy}/{newId}",
                Name = step.Name,
                SenderId = dto.SenderId,
                Status = (GeneralWorkflowStatusType)step.Status,
                ApprovedByIds = step.Approvers,
                HasBeenApprovedByIds = step.Approved,
                ApprovedDates = new(),
                DocumentIds = new() { 14 }
            };

            nodes.Add(node);
            _context.LeaveRequestNodes.Add(node);
        }

        await _context.SaveChangesAsync();
        return _mapper.Map<List<LeaveRequestNodeDTO>>(nodes);
    }

    public async Task<bool> ApproveNodeAsync(int nodeId, int approverId, string? comment = null)
    {
        var node = await _context.LeaveRequestNodes.FindAsync(nodeId);
        if (node == null) return false;

        // Ensure array never null (đề phòng DB đang null)
        node.ApprovedByIds ??= new List<int>();
        node.HasBeenApprovedByIds ??= new List<int>();

        // LOG
        _logger.LogInformation(
            "Approving node {NodeId} by approver {ApproverId} with comment: {Comment}",
            nodeId, approverId, comment
        );

        // Block nếu approverId không có quyền approve
        if (!node.ApprovedByIds.Contains(approverId))
            throw new InvalidOperationException(
                $"Approver {approverId} is not allowed to approve node {nodeId}."
            );

        // Block nếu đã approve rồi
        if (node.HasBeenApprovedByIds.Contains(approverId))
            throw new InvalidOperationException(
                $"Node {nodeId} has already been approved by {approverId}."
            );

        // Thêm vào danh sách đã approve
        node.HasBeenApprovedByIds.Add(approverId);

        node.ApprovedDates ??= new List<DateTime>();
        node.ApprovedDates.Add(DateTime.UtcNow);

        // Check đã đủ approved hay chưa
        var hasNodeAllApproved = Helpers.ArrayHelper.AreArraysEqual(
            node.ApprovedByIds, node.HasBeenApprovedByIds
        );

        if (hasNodeAllApproved)
        {
            node.Status = GeneralWorkflowStatusType.Approved;
            _logger.LogInformation(
                "Node {NodeId} has been fully approved by all required approvers.",
                nodeId
            );
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

    public override async Task<LeaveRequestWorkflowDTO> CreateAsync(
        CreateLeaveRequestWorkflowDTO dto
    )
    {

        var entity = _mapper.Map<LeaveRequestWorkflow>(dto);

        _dbSet.Add(entity);
        await _context.SaveChangesAsync(); // Để có entity.Id

        _logger.LogInformation(
            "Created new LeaveRequestWorkflow with ID {Id} for Employee {EmployeeId}",
            entity.Id,
            entity.EmployeeId
        );

        // ✅ Sinh node
        var nodes = await GenerateStepsAsync(dto, entity);
        var finalDto = _mapper.Map<LeaveRequestWorkflowDTO>(entity);
        // Populate thêm metadata nếu cần
        await PopulateMetadataAsync(finalDto);
        finalDto.LeaveRequestNodes = nodes;

        return finalDto;
    }

    public override async Task<LeaveRequestWorkflowDTO?> UpdateAsync(
        int id,
        UpdateLeaveRequestWorkflowDTO dto
    )
    {
        var workflow = await base.UpdateAsync(id, dto);

        if (workflow == null)
            return null;
        // Populate metadata after update
        await PopulateMetadataAsync(workflow);

        return workflow;
    }

    public override async Task<LeaveRequestWorkflowDTO?> GetByIdAsync(int id)
    {
        var workflow = await base.GetByIdAsync(id);
        if (workflow is null)
        {
            throw new KeyNotFoundException($"LeaveRequestWorkflow with ID {id} not found.");
        }
        await PopulateMetadataAsync(workflow);
        _logger.LogInformation(
            "Retrieved LeaveRequestWorkflow with ID {Id} for Employee {EmployeeId}",
            workflow.WorkAssignedToId, workflow.WorkAssignedToName
        );
        return workflow;
    }

    public override async Task<IEnumerable<LeaveRequestWorkflowDTO>> GetAllAsync()
    {
        var workflows = await base.GetAllAsync();
        var workflowList = workflows.ToList();

        // Populate metadata for each workflow
        foreach (var workflow in workflowList)
        {
            await PopulateMetadataAsync(workflow);
        }


        return workflowList;
    }

    public async Task PopulateMetadataAsync(LeaveRequestWorkflowDTO workflow)
    {
        // Populate EmployeeName
        _logger.LogInformation(
            "Populating metadata for LeaveRequestWorkflow with ID {Id} which has empid {empid} and assignedid {ass}",
            workflow.Id, workflow.EmployeeId, workflow.WorkAssignedToId
        );
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == workflow.EmployeeId);
        var assignee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == workflow.WorkAssignedToId);
        _logger.LogInformation(
            "Found Employee {EmployeeId} with Name {EmployeeName} with assignee {ass}",
            employee?.Id,
            employee?.FirstName + " " + employee?.LastName,
            assignee?.FirstName + " " + assignee?.LastName
        );

        if (employee != null && assignee != null)
        {
            workflow.SenderId = employee.Id;
            workflow.SenderName = employee.FirstName + " " + employee.LastName;
            workflow.EmployeeName = employee.FirstName + " " + employee.LastName;
            workflow.WorkAssignedToName = assignee.FirstName + " " + assignee.LastName;
            workflow.WorkAssignedToPosition = assignee.CompanyInfo.Position ?? "";
            workflow.WorkAssignedToPhone = assignee.CompanyInfo.CompanyPhoneNumber ?? "";
            workflow.WorkAssignedToEmail = assignee.CompanyInfo.CompanyEmail ?? "";
            workflow.WorkAssignedToHomeAdress = assignee.PersonalInfo.Address ?? "";
        }

        _logger.LogInformation(
    "ASSIGNED Employee {name}, {phone}",
    workflow.WorkAssignedToName,
    workflow.WorkAssignedToPhone
);

        // Populate names from IDs
        async Task<List<string>> GetNamesByIdsAsync(List<int> ids)
        {
            return await _context.Employees
                .Where(e => ids.Contains(e.Id))
                .Select(e => e.FirstName + " " + e.MiddleName + " " + e.LastName)
                .ToListAsync();
        }

        workflow.ReceiverNames = await GetNamesByIdsAsync(workflow.ReceiverIds.ToList());
        workflow.HasBeenApprovedByNames = await GetNamesByIdsAsync(workflow.HasBeenApprovedByIds.ToList());
    }
}
