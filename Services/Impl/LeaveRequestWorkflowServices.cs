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

    public async Task<ICollection<LeaveRequestNodeDTO>> GenerateStepsAsync(CreateLeaveRequestWorkflowDTO dto, int workflowId)
    {
        // Get last Id for increment [NOTE: THIS ASSUMES ID IS AUTO-INCREMENTED & DATABASE ALREADY HAS SOMETHING IN]
        var lastId = _context.LeaveRequestNodes.Max(x => x.Id);

        // Calculate total days from StartDate to EndDate
        var totalDays = (dto.EndDate - dto.StartDate).Days + (dto.EndDateDayNightType - dto.StartDateDayNightType) * 0.5;

        if (totalDays < 0)
        {
            throw new ArgumentException("End date must be after start date.");
        }

        // Get managerID
        var managerId = _context.Employees
            .Where(e => e.Id == dto.EmployeeId)
            .Select(e => e.RoleInfo.SupervisorId)
            .FirstOrDefault();


        // var steps = new List<LeaveRequestNode>();
        var steps = new List<(string, int, List<int?>, List<int?>)>();
        steps.Add(("Tạo yêu cầu nghỉ phép", 1, [dto.SenderId], [dto.SenderId]));
        steps.Add(("Quản lý trực thuộc ký", 0, [managerId], []));
        if (totalDays >= 3.0)
        {
            // NOTE: Assuming HR Head has a fixed ID of 11 & there is a manager for it
            var hrHeadId = _context.OrganizationEntities
                .Where(e => e.Id.Equals(11))
                .Select(e => e.ManagerId)
                .FirstOrDefault();

            steps.Add(("Trưởng phòng nhân sự ký", 0, [hrHeadId], []));
        }

        var DirectorExecuteId = _context.OrganizationEntities
            .Where(e => e.Id.Equals(3)) // Assuming 12 is the ID for Executive
            .Select(e => e.ManagerId)
            .FirstOrDefault();
        steps.Add(("Ký xác nhận cuối cùng", 0, [DirectorExecuteId], []));

        var nodes = new List<LeaveRequestNode>();
        for (int i = 0; i < steps.Count; i++)
        {
            var newId = lastId + i + 1;
            // Ensure DraftedByIds are unique
            var node = new LeaveRequestNode
            {
                LeaveRequestWorkflowId = workflowId,
                StepType = (LeaveApprovalStepType)i,
                MainId = "AS-S" + i.ToString() + '-' + DateTime.Now.ToString("dd.MM.yyyy") + '/' + newId.ToString(),
                Name = steps[i].Item1,
                SenderId = dto.SenderId, // Default to 0 if no DraftedByIds
                Status = (GeneralWorkflowStatusType)steps[i].Item2,
                ApprovedByIds = steps[i].Item3.Where(x => x.HasValue).Select(x => x.Value).ToList(),
                HasBeenApprovedByIds = steps[i].Item4.Where(x => x.HasValue).Select(x => x.Value).ToList(),
                ApprovedDates = [],
                DocumentIds = [14]
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

    public override async Task<LeaveRequestWorkflowDTO> CreateAsync(
        CreateLeaveRequestWorkflowDTO dto
    )
    {

        var entity = _mapper.Map<LeaveRequestWorkflow>(dto);

        _dbSet.Add(entity);
        await _context.SaveChangesAsync(); // Để có entity.Id

        // ✅ Sinh node
        var nodes = await GenerateStepsAsync(dto, entity.Id);
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
        if (workflow != null)
        {
            // Populate metadata for the retrieved workflow
            await PopulateMetadataAsync(workflow);
        }
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
        var employee = await _context.Employees.FindAsync(workflow.EmployeeId);
        if (employee != null)
        {
            workflow.EmployeeName = employee.FirstName + " " + employee.LastName;
            workflow.WorkAssignedToName = workflow.EmployeeName;
            workflow.WorkAssignedToPosition = employee.CompanyInfo.Position ?? "";
            workflow.WorkAssignedToPhone = employee.CompanyInfo.CompanyPhoneNumber ?? "";
            workflow.WorkAssignedToEmail = employee.CompanyInfo.CompanyEmail ?? "";
            workflow.WorkAssignedToHomeAdress = employee.PersonalInfo.Address ?? "";
        }

        // Populate names from IDs
        async Task<ICollection<string>> GetNamesByIdsAsync(List<int> ids)
        {
            return await _context.Employees
                .Where(e => ids.Contains(e.Id))
                .Select(e => e.FirstName + " " + e.LastName)
                .ToListAsync();
        }

        workflow.ReceiverNames = await GetNamesByIdsAsync(workflow.ReceiverIds.ToList());
        workflow.HasBeenApprovedByNames = await GetNamesByIdsAsync(workflow.HasBeenApprovedByIds.ToList());
    }
}
