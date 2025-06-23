using portal.DTOs;
using portal.Models;

namespace portal.Mappings;

public abstract class BaseWorkflowNodeProfile<
    TNode,
    TNodeDto,
    TNodeCreateDto,
    TNodeUpdateDto,
    TModel,
    TModelDto,
    TModelCreateDto,
    TModelUpdateDto
> : BaseModelProfile<TModel, TModelDto, TModelCreateDto, TModelUpdateDto>
    where TNode : BaseWorkflowNode
    where TNodeDto : WorkflowNodeDTO
    where TNodeCreateDto : WorkflowNodeCreateDTO
    where TNodeUpdateDto : WorkflowNodeUpdateDTO
    where TModel : BaseModel
    where TModelDto : BaseModelDTO
    where TModelCreateDto : BaseModelCreateDTO
    where TModelUpdateDto : BaseModelUpdateDTO
{
    public BaseWorkflowNodeProfile()
    {
        // Only DTO -> Entity, not the reverse for base
        CreateMap<WorkflowNodeCreateDTO, BaseWorkflowNode>()
            .IncludeBase<BaseModelCreateDTO, BaseModel>();

        CreateMap<WorkflowNodeUpdateDTO, BaseWorkflowNode>()
            .IncludeBase<BaseModelUpdateDTO, BaseModel>();

        // Entity -> DTO
        CreateMap<BaseWorkflowNode, WorkflowNodeDTO>()
            .IncludeBase<BaseModel, BaseModelDTO>()
            .ForMember(dest => dest.WorkflowNodeParticipants, opt => opt.MapFrom(src => src.WorkflowParticipants));

        // Full node mapping
        CreateMap<TNode, TNodeDto>()
            .IncludeBase<BaseWorkflowNode, WorkflowNodeDTO>()
            .IncludeBase<TModel, TModelDto>()
            .ReverseMap();

        CreateMap<TNodeCreateDto, TNode>()
            .IncludeBase<WorkflowNodeCreateDTO, BaseWorkflowNode>()
            .IncludeBase<TModelCreateDto, TModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<TNodeUpdateDto, TNode>()
            .IncludeBase<WorkflowNodeUpdateDTO, BaseWorkflowNode>()
            .IncludeBase<TModelUpdateDto, TModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
