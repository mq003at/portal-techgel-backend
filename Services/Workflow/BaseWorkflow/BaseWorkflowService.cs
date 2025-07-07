using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;

namespace portal.Services;

public abstract class BaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>
    : BaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>,
        IBaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>
    where TModel : BaseWorkflow, new()
    where TReadDTO : BaseWorkflowDTO
    where TCreateDTO : BaseWorkflowCreateDTO
    where TUpdateDTO : BaseWorkflowUpdateDTO
    where TNodeModel : BaseWorkflowNode
{
    protected new readonly ApplicationDbContext _context;
    protected new readonly IMapper _mapper;
    protected new readonly ILogger<
        BaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>
    > _logger;

    public BaseWorkflowService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<BaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>> logger
    )
        : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    //Delete
    public virtual async Task<bool> DeleteWorkflowAsync(int id)
    {
        TModel workflow =
            await _context.Set<TModel>().FindAsync(id)
            ?? throw new Exception("Không thấy. Có thể đã bị xóa hoặc không tồn tại.");

        if (workflow.Status != GeneralWorkflowStatusType.DRAFT)
            throw new InvalidOperationException(
                "Chỉ có thể xóa các quy trình làm việc ở trạng thái nháp(khi chưa có ai ký)."
            );

        _context.Set<TModel>().Remove(workflow);

        await _context.SaveChangesAsync();
        return true;
    }

    // Update
    public virtual async Task<bool> UpdateWorkflowAsync(int id, TUpdateDTO dto)
    {
        TModel workflow =
            await _context.Set<TModel>().FindAsync(id)
            ?? throw new KeyNotFoundException("Không tìm thấy quy trình. Vui lòng kiểm tra lại.");

        if (workflow.Status != GeneralWorkflowStatusType.DRAFT)
            throw new InvalidOperationException(
                "Chỉ có thể cập nhật các quy trình làm việc ở trạng thái nháp (khi chưa có ai ký)."
            );

        _ = await _context.Employees.FindAsync(workflow.SenderId)
            ?? throw new KeyNotFoundException("Không tìm thấy người gửi. Vui lòng kiểm tra lại.");

        _context.Set<TModel>().Update(workflow);
        await _context.SaveChangesAsync();
        return true;
    }

    // Get all workflows based on Sender ID
    public async Task<List<TReadDTO>> GetAllByEmployeeIdAsync(int id)
    {
        var workflows = await _context.Set<TModel>().Where(w => w.SenderId == id).ToListAsync();

        var workflowDtos = _mapper.Map<List<TReadDTO>>(workflows);

        return workflowDtos;
    }

    // Get All
    public virtual async Task<IEnumerable<TReadDTO>> GetAllWorkflowsAsync()
    {
        var workflowDtos = await _mapper
            .ProjectTo<TReadDTO>(_context.Set<TModel>().AsNoTracking())
            .ToListAsync();
        return workflowDtos;
    }

}
