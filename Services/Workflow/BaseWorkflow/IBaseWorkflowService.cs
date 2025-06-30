namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface IBaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>
    : IBaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>
    where TModel : BaseWorkflow
    where TReadDTO : BaseWorkflowDTO
    where TCreateDTO : BaseWorkflowCreateDTO
    where TUpdateDTO : BaseWorkflowUpdateDTO
    where TNodeModel : BaseWorkflowNode
{
    Task<bool> DeleteWorkflowAsync(int id);
}