namespace portal.Services;

using System.Text.Json;
using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Extensions;
using portal.Helpers;
using portal.Models;
using Serilog;

public class LeaveRequestWorkflowService
    : BaseWorkflowService<
        LeaveRequestWorkflow,
        LeaveRequestWorkflowDTO,
        LeaveRequestWorkflowCreateDTO,
        LeaveRequestWorkflowUpdateDTO,
        LeaveRequestNode
    >,
        ILeaveRequestWorkflowService
{
    private readonly IFileStorageService _storage;
    private readonly DocumentOptions _docOpts;
    private readonly string _basePath;
    private readonly ICapPublisher _capPublisher;
    private readonly IEmployeeService _employeeService;

    public LeaveRequestWorkflowService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<LeaveRequestWorkflowService> logger,
        IOptions<DocumentOptions> docOpts,
        IFileStorageService storage,
        ICapPublisher capPublisher,
        IEmployeeService employeeService
    )
        : base(context, mapper, logger)
    {
        _docOpts = docOpts.Value;
        _storage = storage;
        _basePath = AppDomain.CurrentDomain.BaseDirectory;
        _capPublisher = capPublisher;
        _employeeService = employeeService;
    }

    public new async Task<List<LeaveRequestWorkflowDTO>> GetAllByEmployeeIdAsync(int id)
    {
        var workflows = await _context
            .LeaveRequestWorkflows.Where(w => w.EmployeeId == id)
            .Include(w => w.LeaveRequestNodes)
            .ToListAsync();

        var workflowDtos = _mapper.Map<List<LeaveRequestWorkflowDTO>>(workflows);

        var allAssigneeIds = workflowDtos.SelectMany(wf => wf.AssigneeDetails).Distinct().ToList();

        var nameMap = await _employeeService.GetEmployeeNamesByIdsAsync(allAssigneeIds);

        foreach (var wf in workflowDtos)
        {
            wf.AssigneeNames = wf
                .AssigneeDetails.Select(id => nameMap.GetValueOrDefault(id) ?? string.Empty)
                .ToList();
        }

        return workflowDtos;
    }

    public async Task<List<LeaveRequestNodeDTO>> GenerateNodesAsync(
        LeaveRequestWorkflowCreateDTO dto,
        LeaveRequestWorkflow workflow
    )
    {
        // Get the last node ID (handle empty table)
        var lastId = await _context.LeaveRequestNodes.AnyAsync()
            ? await _context.LeaveRequestNodes.MaxAsync(x => x.Id)
            : 0;

        // Calculate number of leave days (min 0.5)
        var totalDays = DateHelper.CalculateLeaveDays(
            dto.StartDate,
            (int)dto.StartDateDayNightType,
            dto.EndDate,
            (int)dto.EndDateDayNightType
        );

        if (totalDays < 0.4)
            throw new ArgumentException(
                "End date must be after start date and at least half a day apart."
            );

        var employeeId = dto.EmployeeId;
        var senderId = dto.SenderId;
        var assigneeDetails = dto.AssigneeDetails;
        Employee employee =
            await _context
                .Employees.Include(e => e.CompanyInfo)
                .Include(e => e.ScheduleInfo)
                .Include(e => e.Supervisor)
                .Include(e => e.DeputySupervisor)
                .FirstOrDefaultAsync(e => e.Id == employeeId)
            ?? throw new InvalidOperationException($"Employee {dto.EmployeeId} not found.");

        // Null checks for employee properties
        CompanyInfo companyInfo =
            employee.CompanyInfo
            ?? throw new InvalidOperationException(
                $"Nhân viên chưa được cập nhật Thông tin Công ty. Vui lòng liên hệ bộ phận HR."
            );
        Employee supervisor =
            employee.Supervisor
            ?? throw new InvalidOperationException(
                $"Không tìm thấy Quản lý trực thuộc cho nhân viên này. Xin vui lòng liên hệ bộ phận HR."
            );
        Employee? deputySupervisor = employee.DeputySupervisor;
        // ScheduleInfo scheduleInfo = employee.ScheduleInfo ?? throw new InvalidOperationException($"ScheduleInfo for employee {dto.EmployeeId} not found.");
        List<Employee> assignees = await _context
            .Employees.Where(e => assigneeDetails.Contains(e.Id))
            .ToListAsync();

        List<string> assigneeNames = (
            await _employeeService.GetEmployeeNamesByIdsAsync(assigneeDetails)
        ).Values.ToList();

        // IF isOnProbation, DO NOT reduce AnnualLeave or CompensatoryLeave
        var isOnProbation = companyInfo.IsOnProbation;
        if (
            isOnProbation
            && (
                dto.LeaveApprovalCategory == LeaveApprovalCategory.AnnualLeave
                || dto.LeaveApprovalCategory == LeaveApprovalCategory.CompensatoryLeave
            )
        )
        {
            throw new InvalidOperationException(
                "Không thể xin nghỉ phép hay nghỉ bù khi đang trong giai đoạn thực tập."
            );
        }

        // Building Leave information using Employee navigation properties
        var compensatoryLeaveInit = companyInfo.CompensatoryLeaveTotalDays;
        var annualLeaveInit = companyInfo.AnnualLeaveTotalDays;
        var finalEmployeeAnnualLeaveTotalDays = annualLeaveInit;
        var finalEmployeeCompensatoryLeaveTotalDays = compensatoryLeaveInit;

        workflow.TotalDays = (double)totalDays;

        // calculate for the 2 specific leaves
        if (dto.LeaveApprovalCategory == LeaveApprovalCategory.CompensatoryLeave)
        {
            if (compensatoryLeaveInit < totalDays)
            {
                throw new InvalidOperationException(
                    "Không đủ ngày nghỉ bù. Vui lòng liên hệ với quản trị viên để kiểm tra."
                );
            }
            finalEmployeeCompensatoryLeaveTotalDays = compensatoryLeaveInit - totalDays;
            _logger.LogError("finalDays: {fdays}", finalEmployeeCompensatoryLeaveTotalDays);
        }
        else if (dto.LeaveApprovalCategory == LeaveApprovalCategory.AnnualLeave)
        {
            if (annualLeaveInit < totalDays)
            {
                throw new InvalidOperationException(
                    "Không đủ ngày nghỉ phép. Vui lòng liên hệ với quản trị viên để kiểm tra."
                );
            }
            finalEmployeeAnnualLeaveTotalDays = annualLeaveInit - totalDays;
            _logger.LogError("finalDays: {fdays}", finalEmployeeAnnualLeaveTotalDays);
        }

        // For other types of leave, we do not modify annual leave or compensatory leave
        workflow.FinalEmployeeCompensatoryLeaveTotalDays = finalEmployeeCompensatoryLeaveTotalDays;
        workflow.FinalEmployeeAnnualLeaveTotalDays = finalEmployeeAnnualLeaveTotalDays;
        workflow.EmployeeCompensatoryLeaveTotalDays = compensatoryLeaveInit;
        workflow.EmployeeAnnualLeaveTotalDays = annualLeaveInit;
        workflow.EmployeeId = employeeId;
        workflow.Reason = dto.Reason;
        workflow.LeaveApprovalCategory = dto.LeaveApprovalCategory;
        workflow.Employee = employee;
        workflow.AssigneeDetails = assigneeDetails;
        workflow.AssigneeNames = assigneeNames;

        // Build up receiver IDs for workflow
        // Compose workflow steps


        // Initiate nodes creation: 2 nodes for now, first one has 2 participants, second one has 2 participants
        var steps = new List<(
            string Name,
            LeaveApprovalStepType StepType,
            int Status,
            List<WorkflowNodeParticipant> nodeParticipants
        )>
        {
            (
                "Tạo yêu cầu nghỉ phép",
                LeaveApprovalStepType.CreateForm,
                1,
                new List<WorkflowNodeParticipant> { }
            ),
            (
                "Quản lý trực thuộc / gián tiếp ký",
                LeaveApprovalStepType.ExecutiveApproval,
                0,
                new List<WorkflowNodeParticipant> { }
            ),
        };

        // Build and add nodes
        var nodes = new List<LeaveRequestNode>();
        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];

            var node = new LeaveRequestNode
            {
                WorkflowId = workflow.Id,
                StepType = step.StepType,
                MainId = $"AS-S{i}-{DateTime.Now:dd.MM.yyyy}",
                Name = step.Name,
                Status = (GeneralWorkflowStatusType)step.Status,
                WorkflowNodeParticipants = step.nodeParticipants,
                Description = step.Name,
            };

            nodes.Add(node);
        }
        _context.LeaveRequestNodes.AddRange(nodes);
        await _context.SaveChangesAsync();
        _logger.LogError("Node [0] ID: {id}", nodes[0].Id);

        var participants = new List<(
            string EmpId,
            int WorkflowNodeId,
            WorkflowParticipantRoleType RaciRole,
            LeaveApprovalStepType StepType,
            int Order,
            DateTime? ApprovalDate,
            DateTime? ApprovalStartDate,
            DateTime? ApprovalDeadline,
            bool? IsApproved,
            ApprovalStatusType ApprovalStatus,
            TimeSpan? TAT
        )>
        {
            (
                employee.MainId,
                nodes[0].Id,
                WorkflowParticipantRoleType.RESPONSIBLE,
                LeaveApprovalStepType.CreateForm,
                0,
                DateTime.UtcNow,
                DateTime.UtcNow,
                DateTime.UtcNow,
                true,
                ApprovalStatusType.APPROVED,
                TimeSpan.Zero
            ),
            (
                supervisor.MainId,
                nodes[1].Id,
                WorkflowParticipantRoleType.ACCOUNTABLE,
                LeaveApprovalStepType.ExecutiveApproval,
                0,
                null,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(48),
                false,
                ApprovalStatusType.PENDING,
                null
            )
        };

        if (deputySupervisor != null)
        {
            participants.Add(
                (
                    deputySupervisor.MainId,
                    nodes[1].Id,
                    WorkflowParticipantRoleType.ACCOUNTABLE,
                    LeaveApprovalStepType.ExecutiveApproval,
                    1,
                    null,
                    DateTime.UtcNow.AddHours(48),
                    DateTime.UtcNow.AddHours(96),
                    false,
                    ApprovalStatusType.NOT_APPLICABLE,
                    null
                )
            );
        }

        // foreach assignees in here
        int index = 0;
        foreach (var assignee in assignees)
        {
            participants.Add(
                (
                    assignee.MainId,
                    nodes[0].Id,
                    WorkflowParticipantRoleType.INFORMED,
                    LeaveApprovalStepType.CreateForm,
                    index + 1,
                    null,
                    null,
                    null,
                    false,
                    ApprovalStatusType.NOT_APPLICABLE,
                    null
                )
            );
            index++;
        }

        // Convert the participants above to WorkflowNodeParticipant objects
        var WorkflowNodeParticipants = new List<WorkflowNodeParticipant>();
        foreach (var participant in participants)
        {
            WorkflowNodeParticipants.Add(
                new WorkflowNodeParticipant
                {
                    EmployeeId =
                        _context.Employees.FirstOrDefault(e => e.MainId == participant.EmpId)?.Id
                        ?? 0,
                    Order = participant.Order,
                    RaciRole = participant.RaciRole,
                    ApprovalDate = participant.ApprovalDate,
                    ApprovalStartDate = participant.ApprovalStartDate,
                    ApprovalDeadline = participant.ApprovalDeadline,
                    ApprovalStatus = participant.ApprovalStatus,
                    TAT = participant.TAT,
                    WorkflowNodeType = "LeaveRequest",
                    WorkflowNodeId = participant.WorkflowNodeId,
                    WorkflowId = workflow.Id
                }
            );

            _logger.LogError(
                "NodeID: {NodeId}, StepType: {StepType}, Order: {Order}, EmployeeId: {EmployeeId}",
                participant.WorkflowNodeId,
                participant.StepType,
                participant.Order,
                participant.EmpId
            );
        }
        _context.WorkflowNodeParticipants.AddRange(WorkflowNodeParticipants);

        nodes[0].WorkflowNodeParticipants = new List<WorkflowNodeParticipant>
        {
            WorkflowNodeParticipants[0] // Employee who created the request
        };

        foreach (var participant in WorkflowNodeParticipants)
        {
            if (participant.WorkflowNodeId == nodes[1].Id)
            {
                nodes[1].WorkflowNodeParticipants.Add(participant);
            }
            else if (participant.WorkflowNodeId == nodes[0].Id)
            {
                nodes[0].WorkflowNodeParticipants.Add(participant);
            }
        }

        workflow.ParticipantIds = new List<int> { employee.Id, supervisor.Id };
        if (deputySupervisor != null)
        {
            workflow.ParticipantIds.Add(deputySupervisor.Id);
        }

        // Create notification events for all participants
        var employeeIdsToNotify = new List<int> { employee.Id, supervisor.Id };
        employeeIdsToNotify.AddRange(assigneeDetails);
        employeeIdsToNotify = employeeIdsToNotify.Distinct().ToList();

        _logger.LogError("Employee IDs to notify: {Ids}", string.Join(", ", employeeIdsToNotify));

        var events = new List<CreateEvent>();
        employeeIdsToNotify.ForEach(id =>
        {
            events.Add(
                new CreateEvent
                {
                    WorkflowId = workflow.MainId.ToString(),
                    WorkflowType = "Nghỉ phép",
                    EmployeeId = id,
                    ApproverName = supervisor.GetDisplayName(),
                    TriggeredBy = employee.GetDisplayName(),
                    CreatedAt = DateTime.UtcNow,
                    Status = "CREATED",
                    EmployeeName = employee.GetDisplayName(),
                    AssigneeDetails = string.Join(", ", assignees.Select(a => a.GetDisplayName()))
                }
            );
        });

        foreach (var @event in events)
        {
            _logger.LogError(
                "Publishing event for workflow creation: {@Event}",
                JsonSerializer.Serialize(@event)
            );
            await _capPublisher.PublishAsync("leaverequest.workflow.created", @event);
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<List<LeaveRequestNodeDTO>>(nodes);
    }

    public async Task<IEnumerable<LeaveRequestNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId)
    {
        var nodes = await _context
            .LeaveRequestNodes.Where(n => n.WorkflowId == workflowId)
            .ToListAsync();

        var nodeIds = nodes.Select(n => n.Id).ToList();

        var participants = await _context
            .WorkflowNodeParticipants.Where(p => p.WorkflowNodeType == "LeaveRequest")
            .ToListAsync();

        foreach (var node in nodes)
        {
            node.WorkflowNodeParticipants = participants
                .Where(p => p.WorkflowNodeId == node.Id)
                .ToList();
        }

        var dtos = _mapper.Map<IEnumerable<LeaveRequestNodeDTO>>(nodes);
        _logger.LogInformation(
            "first node has {ParticipantCount} participants.",
            dtos.First().WorkflowNodeParticipants.Count()
        );
        return dtos;
    }

    public async Task<bool> FinalizeIfCompleteAsync(int workflowId)
    {
        LeaveRequestWorkflow workflow =
            await _context
                .LeaveRequestWorkflows.Include(w => w.LeaveRequestNodes)
                .FirstOrDefaultAsync(w => w.Id == workflowId)
            ?? throw new InvalidOperationException($"Không tìm thấy đơn.");

        if (workflow.IsDocumentGenerated)
        {
            throw new InvalidOperationException(
                "Quy trình đã có tài liệu đính kèm, không cần phải lấy bản vật lý lại."
            );
        }
        WorkflowNodeParticipant finalParticipant =
            _context
                .Set<WorkflowNodeParticipant>()
                .Where(p =>
                    p.WorkflowId == workflowId
                    && p.WorkflowNodeType == "LeaveRequest"
                    && p.ApprovalStatus == ApprovalStatusType.APPROVED
                )
                .OrderByDescending(p => p.Id)
                .FirstOrDefault()
            ?? throw new InvalidOperationException(
                "Quy trình chưa được phê duyệt hoàn tất, không thể lấy bản vật lý."
            );

        Employee employee =
            await _context
                .Employees.Include(e => e.CompanyInfo)
                .Include(e => e.Signature)
                .FirstOrDefaultAsync(e => e.Id == workflow.EmployeeId)
            ?? throw new InvalidOperationException($"Employee {workflow.EmployeeId} not found.");

        Employee approver =
            await _context
                .Employees.Include(e => e.CompanyInfo)
                .Include(e => e.Signature)
                .FirstOrDefaultAsync(e => e.Id == finalParticipant.EmployeeId)
            ?? throw new InvalidOperationException(
                $"Employee {finalParticipant.EmployeeId} not found."
            );

        var result = await GenerateLeaveRequestFinalDocument(employee, approver, workflow);

        return result;
    }

    public override async Task<bool> DeleteAsync(int id)
    {
        // Check if the workflow exists
        var workflow = await _dbSet.FindAsync(id);
        if (workflow == null)
        {
            throw new KeyNotFoundException(
                $"Không tìm thấy đơn nghỉ. Chắc là bạn đã xóa nó. Vui lòng quay lại trang trước để kiểm tra lại."
            );
        }

        // Check if the workflow is in a state that allows deletion
        if (workflow.Status != GeneralWorkflowStatusType.DRAFT)
        {
            throw new InvalidOperationException(
                "Chỉ có những đơn từ nào chưa được ký duyệt mới có thể xóa."
            );
        }

        // Delete all associated nodes and participants
        var nodes = await _context.LeaveRequestNodes.Where(n => n.WorkflowId == id).ToListAsync();
        _context.LeaveRequestNodes.RemoveRange(nodes);

        var participants = await _context
            .WorkflowNodeParticipants.Where(p =>
                p.WorkflowId == id && p.WorkflowNodeType == "LeaveRequest"
            )
            .ToListAsync();
        _context.WorkflowNodeParticipants.RemoveRange(participants);

        // Delete the workflow itself
        _dbSet.Remove(workflow);
        await _context.SaveChangesAsync();
        return true;
    }

    public override async Task<LeaveRequestWorkflowDTO> CreateAsync(
        LeaveRequestWorkflowCreateDTO dto
    )
    {
        // check for existing pending workflow
        // var existingWorkflow = await _context
        //     .LeaveRequestWorkflows.Where(w =>
        //         w.EmployeeId == dto.EmployeeId
        //         && (
        //             w.Status == GeneralWorkflowStatusType.PENDING
        //             || w.Status == GeneralWorkflowStatusType.DRAFT
        //         )
        //     )
        //     .FirstOrDefaultAsync();

        // if (existingWorkflow != null)
        // {
        //     throw new InvalidOperationException(
        //         "Đã có 1 đơn nghỉ phép đang chờ xử lý cho nhân viên này. Vui lòng đợi hoặc hủy đơn trước."
        //     );
        // }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Map DTO to entity
            var entity = _mapper.Map<LeaveRequestWorkflow>(dto);

            // Fetch employee
            var employee =
                await _context
                    .Employees.Include(e => e.Supervisor)
                    .Include(e => e.DeputySupervisor)
                    .FirstOrDefaultAsync(e => e.Id == dto.EmployeeId)
                ?? throw new InvalidOperationException(
                    $"Không tìm thấy nhân viên {dto.EmployeeId}."
                );

            // Generate workflow description
            entity.Description =
                $"Hồ sơ nghỉ phép nhân viên {employee.GetDisplayName()} từ {dto.StartDate:dd/MM/yyyy} đến {dto.EndDate:dd/MM/yyyy}. Người ký: {employee.Supervisor?.GetDisplayName()} và {employee.DeputySupervisor?.GetDisplayName()}";

            // Check assignee details if employeeID exists
            if (dto.AssigneeDetails == null || dto.AssigneeDetails.Count == 0)
            {
                throw new InvalidOperationException(
                    "Không có người được chỉ định cho đơn nghỉ phép này. Vui lòng thêm người được chỉ định."
                );
            }

            var existingIds = await _context
                .Employees.Where(e => dto.AssigneeDetails.Contains(e.Id))
                .Select(e => e.Id)
                .ToListAsync();

            // Step 3: Compare and find invalid IDs
            var missingIds = dto.AssigneeDetails.Except(existingIds).ToList();

            if (missingIds.Any())
            {
                var missingStr = string.Join(", ", missingIds);
                throw new InvalidOperationException(
                    $"Người được chỉ định không hợp lệ (ID: {missingStr}). Vui lòng kiểm tra lại."
                );
            }

            // Save workflow first to get its Id
            _context.LeaveRequestWorkflows.Add(entity);
            await _context.SaveChangesAsync();

            // Generate workflow nodes with entity.Id now available
            var nodes = await GenerateNodesAsync(dto, entity);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Map to DTO after commit
            var finalDto = _mapper.Map<LeaveRequestWorkflowDTO>(entity);
            finalDto.LeaveRequestNodes = _mapper.Map<List<LeaveRequestNodeDTO>>(nodes);

            return finalDto;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public override async Task<LeaveRequestWorkflowDTO?> UpdateAsync(
        int id,
        LeaveRequestWorkflowUpdateDTO dto
    )
    {
        LeaveRequestWorkflow workflow =
            _dbSet.FirstOrDefaultAsync(wf => wf.Id == id).Result
            ?? throw new KeyNotFoundException(
                $"Không tìm thấy đơn nghỉ. Vui lòng quay lại trang trước để kiểm tra lại."
            );

        if (workflow.Status != GeneralWorkflowStatusType.DRAFT)
        {
            throw new InvalidOperationException(
                "Chỉ có những đơn nghỉ phép chưa được ký duyệt mới có thể cập nhật."
            );
        }
        else
            workflow = _mapper.Map(dto, workflow);

        // Update the workflow
        _context.Entry(workflow).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated workflow with ID {Id}", id);
        return _mapper.Map<LeaveRequestWorkflowDTO>(workflow);
    }

    public override async Task<LeaveRequestWorkflowDTO?> GetByIdAsync(int id)
    {
        // Step 1: Get workflow and its nodes
        LeaveRequestWorkflow workflow =
            await _dbSet.Include(wf => wf.LeaveRequestNodes).FirstOrDefaultAsync(wf => wf.Id == id)
            ?? throw new KeyNotFoundException($"Workflow with ID {id} not found.");

        // Step 2: Load all participants in one query
        var nodeIds = workflow.LeaveRequestNodes.Select(n => n.Id).ToList();

        var participants = await _context
            .WorkflowNodeParticipants.Where(p =>
                nodeIds.Contains(p.WorkflowNodeId) && p.WorkflowNodeType == "LeaveRequest"
            )
            .Include(p => p.Employee)
            .ToListAsync();

        // Step 3: Assign participants to nodes
        foreach (var node in workflow.LeaveRequestNodes)
        {
            node.WorkflowNodeParticipants = participants
                .Where(p => p.WorkflowNodeId == node.Id)
                .ToList();
        }

        workflow.DocumentAssociations = await _context
            .DocumentAssociations.Where(da =>
                da.EntityId == workflow.Id && da.EntityType == "LeaveRequest"
            )
            .Include(da => da.Document) // Eager load the Document
            .Select(da => da.Document)
            .ToListAsync();

        return _mapper.Map<LeaveRequestWorkflowDTO>(workflow);
    }

    public async Task<LeaveRequestNodeDTO?> GetAllWorkflowByEmployeeId(int employeeId)
    {
        var workflow = await _context
            .LeaveRequestWorkflows.Include(w => w.LeaveRequestNodes)
            .FirstOrDefaultAsync(w => w.EmployeeId == employeeId);

        if (workflow == null)
            return null;

        var workflowDto = _mapper.Map<LeaveRequestNodeDTO>(workflow);
        return workflowDto;
    }

    public override async Task<IEnumerable<LeaveRequestWorkflowDTO>> GetAllAsync()
    {
        var workflows = await base.GetAllAsync();
        var workflowList = workflows.ToList();

        return workflowList;
    }

    private async Task<string> GenerateDocumentsAsync(int workflowId, int approverId)
    {
        LeaveRequestWorkflow workflow =
            await _context
                .LeaveRequestWorkflows.Include(w => w.LeaveRequestNodes)
                .FirstOrDefaultAsync(w => w.Id == workflowId)
            ?? throw new InvalidOperationException($"Workflow {workflowId} not found.");

        // Get the approver's information
        Employee approver =
            await _context.Employees.FindAsync(approverId)
            ?? throw new InvalidOperationException($"Approver {approverId} not found.");
        // Get the employee's information
        Employee employee =
            await _context.Employees.FindAsync(workflow.EmployeeId)
            ?? throw new InvalidOperationException($"Employee {workflow.EmployeeId} not found.");
        return "";
    }

    private async Task<List<string>> GetNamesByIdsAsync(List<int> ids)
    {
        return await _context
            .Employees.Where(e => ids.Contains(e.Id))
            .Select(e => e.LastName + " " + e.MiddleName + " " + e.FirstName)
            .ToListAsync();
    }

    // Add document only at the end of the workflow. The newly created document will be attached to the workflow itself, not the nodes
    public async Task<bool> GenerateLeaveRequestFinalDocument(
        Employee employee,
        Employee approver,
        LeaveRequestWorkflow workflow
    )
    {
        var assignees = await _context
            .Employees.Where(e => workflow.AssigneeDetails.Contains(e.Id))
            .Select(e => new
            {
                Employee = e,
                Name = (e.LastName + " " + e.MiddleName + " " + e.FirstName).Trim()
            })
            .ToListAsync();

        List<Employee> assigneeList = assignees.Select(a => a.Employee).ToList();
        List<string> assigneeNames = assignees.Select(a => a.Name).ToList();
        string assigneeNameToStrings =
            assigneeNames.Count > 0 ? string.Join(", ", assigneeNames) : "Không có người nhận";

        var templateDocMetadata = await _context.Documents.FirstOrDefaultAsync(d =>
            d.TemplateKey == "LeaveRequest"
        );

        var today = DateTime.UtcNow;

        if (templateDocMetadata == null)
            throw new InvalidOperationException("Template document for 'LeaveRequest' not found.");

        // Get that file from the VPS, and all the related signatures
        var newDoc = await _storage.DownloadAsync(templateDocMetadata.Url);

        string employeeSignaturePath =
            employee.Signature?.StoragePath
            ?? throw new InvalidOperationException(
                $"Employee {employee.Id} does not have a signature."
            );
        string approverSignaturePath =
            approver.Signature?.StoragePath
            ?? throw new InvalidOperationException(
                $"Approver {approver.Id} does not have a signature."
            );

        MemoryStream employeeSignature = await FileHandling.ToMemoryStreamAsync(
            await _storage.DownloadAsync(employee.Signature.StoragePath)
        );

        MemoryStream approverSignature = await FileHandling.ToMemoryStreamAsync(
            await _storage.DownloadAsync(approver.Signature.StoragePath)
        );

        bool isImgValid = FileHandling.IsPngHeader(employeeSignature);
        _logger.LogInformation("Employee signature is valid: {IsValid}", isImgValid);

        var Location = "6_Noi_Chinh";
        var newFileName =
            $"{workflow.Id}-{today:yyyy-MM-dd}-{Location}-DN-{employee.MainId}-v01{".docx"}";

        var newTargetPath = Path.Combine(
                "erp",
                "documents",
                Location,
                "Ho_So",
                "Nghi_Phep",
                newFileName
            )
            .Replace("\\", "/");

        // Filling in steps
        var placeholders = new Dictionary<string, (string Text, bool IsBold)>
        {
            // English
            ["fullName"] = (
                $"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(),
                false
            ),
            ["department"] = (employee.CompanyInfo?.Department ?? "", false),
            ["position"] = (employee.CompanyInfo?.Position ?? "", false),
            ["startDate"] = (workflow.StartDate.ToString("dd/MM/yyyy"), false),
            ["endDate"] = (workflow.EndDate.ToString("dd/MM/yyyy"), false),
            ["reason"] = (workflow.Reason ?? "", false),
            ["leaveRequestStartDate"] = (
                (workflow.StartDateDayNightType == 0 ? "Sáng " : "Chiều ")
                    + workflow.StartDate.ToString("dd/MM/yyyy"),
                false
            ),
            ["leaveRequestEndDate"] = (
                (workflow.EndDateDayNightType == 0 ? "Sáng " : "Chiều ")
                    + workflow.EndDate.ToString("dd/MM/yyyy"),
                false
            ),
            ["totalDaysTop"] = (workflow.TotalDays.ToString(), false),
            ["totalDaysBox"] = (workflow.TotalDays.ToString(), false),
            ["finalAnnualTotal"] = (workflow.FinalEmployeeAnnualLeaveTotalDays.ToString(), false),

            ["employeeName"] = (
                $"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(),
                false
            ),
            ["employeeId"] = (employee.Id.ToString(), false),
            ["employeeSignDate"] = (today.ToString("dd/MM/yyyy"), false),
            ["assigneeDetails"] = (assigneeNameToStrings, false),
            ["notes"] = (workflow.Notes ?? "", false),

            ["empAnnualTotal"] = (
                employee.CompanyInfo?.AnnualLeaveTotalDays.ToString() ?? "",
                false
            ),
            ["empCompensatoryTotal"] = (
                employee.CompanyInfo?.CompensatoryLeaveTotalDays.ToString() ?? "",
                false
            ),
            ["totalAnnualDays"] = (workflow.TotalDays.ToString(), false),
            ["finalCompensatoryTotal"] = (
                workflow.FinalEmployeeCompensatoryLeaveTotalDays.ToString(),
                false
            ),

            ["employeeFullName"] = (
                $"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(),
                false
            ),
            ["employeeFullNameBottom"] = (
                $"{employee.LastName} {employee.MiddleName} {employee.FirstName}".Trim(),
                false
            ),
            ["employeeSignDate"] = (workflow.CreatedAt.ToString("dd/MM/yyyy"), false),

            ["approverPosition"] = (approver.CompanyInfo?.Position?.ToUpper() ?? "", true), // BOLD
            ["approverFullName"] = (
                $"{approver.LastName} {approver.MiddleName} {approver.FirstName}".Trim(),
                false
            ),
            ["approverSignDate"] = (today.ToString("dd/MM/yyyy"), false),

            ["type"] = (
                workflow.LeaveApprovalCategory switch
                {
                    LeaveApprovalCategory.AnnualLeave => "Nghỉ phép có lương",
                    LeaveApprovalCategory.UnpaidLeave => "Nghỉ phép không lương",
                    LeaveApprovalCategory.SickLeave => "Nghỉ ốm",
                    LeaveApprovalCategory.MaternityLeave => "Nghỉ thai sản",
                    LeaveApprovalCategory.PaternityLeave => "Nghỉ tang",
                    LeaveApprovalCategory.CompensatoryLeave => "Nghỉ bù",
                    _ => workflow.LeaveApprovalCategory.ToString()
                },
                false
            )
        };

        MemoryStream newMemoryDoc = await FileHandling.ToMemoryStreamAsync(newDoc);

        WordBookmarkReplacer.ReplacePlaceholders(newMemoryDoc, placeholders);

        newMemoryDoc = await WordImageInserter.InsertImageAtBookmarkAsync(
            newMemoryDoc,
            "employeeSignature",
            employeeSignature
        );

        newMemoryDoc = await WordImageInserter.InsertImageAtBookmarkAsync(
            newMemoryDoc,
            "approverSignature",
            approverSignature
        );

        // If filling in is completed, upload and make a new record of metadata
        var pathAfterUpload = await _storage.UploadAsync(newMemoryDoc, newTargetPath);
        if (string.IsNullOrEmpty(pathAfterUpload))
            throw new InvalidOperationException("Tải file lên thất bại. Server bị quá tải.");

        var newMetadata = new Models.Document
        {
            Name = newFileName,
            Url = pathAfterUpload,
            TemplateKey = null,
            Description =
                $"Mẫu đơn nghỉ phép của {employee.LastName} {employee.MiddleName} {employee.FirstName}",
            FileExtension = ".docx",
            SizeInBytes = newDoc.Length,
            Status = DocumentStatusEnum.DRAFT,
            Version = "1.0",
            Tag = templateDocMetadata.Tag,
            Category = DocumentCategoryEnum.FORM,
            Location = Location
        };

        _logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(workflow.Id));

        DocumentAssociation newDocumentAssociation = new DocumentAssociation
        {
            Document = newMetadata,
            EntityType = "LeaveRequest",
            EntityId = workflow.Id, // Associate with the workflow ID
        };

        workflow.IsDocumentGenerated = true;
        _context.Entry(workflow).State = EntityState.Modified;
        _context.Documents.Add(newMetadata);
        _context.DocumentAssociations.Add(newDocumentAssociation);
        await _context.SaveChangesAsync();

        return true;
    }

    public class Attachment
    {
        public string FileName { get; set; } = "";
        public string ContentType { get; set; } =
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
    }
}
