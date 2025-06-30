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
    public async Task<bool> DeleteWorkflowAsync(int id)
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
}
