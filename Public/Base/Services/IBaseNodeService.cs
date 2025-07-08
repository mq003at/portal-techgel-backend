namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface IBaseNodeService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TWorkflowModel>
    : IBaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>
    where TModel : BaseWorkflowNode
    where TReadDTO : WorkflowNodeDTO
    where TCreateDTO : WorkflowNodeCreateDTO
    where TUpdateDTO : WorkflowNodeUpdateDTO
    where TWorkflowModel : BaseWorkflow
{
    Task<bool> ApproveAsync(int nodeId, ApproveWithCommentDTO dto);
    Task<bool> RejectAsync(int nodeId, RejectDTO dto);
}
