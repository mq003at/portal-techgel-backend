using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;

public abstract class BaseWorkflowProfile<TWorkflow, TWorkflowDto, TModel, TDto> : BaseModelProfile<TModel, TDto>
    where TWorkflow : BaseWorkflow
    where TWorkflowDto : BaseWorkflowDTO<TWorkflow>
    where TModel : BaseModel
    where TDto : BaseDTO<TModel>
{
    public BaseWorkflowProfile()
    {
        CreateMap<TWorkflow, TWorkflowDto>(MemberList.Source)
            .IncludeBase<TModel, TDto>();
        CreateMap<TWorkflowDto, TWorkflow>(MemberList.Destination)
            .IncludeBase<TDto, TModel>();
    }
}