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
    where TNodeCreateDto : BaseModelCreateDTO
    where TNodeUpdateDto : BaseModelUpdateDTO
    where TModel : BaseModel
    where TModelDto : BaseModelDTO
    where TModelCreateDto : BaseModelCreateDTO
    where TModelUpdateDto : BaseModelUpdateDTO
{
    public BaseWorkflowNodeProfile()
    {
        // Entity <-> DTO (allow navigation, reverse when safe)
        CreateMap<TNode, TNodeDto>()
            .IncludeBase<TModel, TModelDto>()
            .ReverseMap();

        // Create DTO -> Entity
        CreateMap<TNodeCreateDto, TNode>()
            .IncludeBase<TModelCreateDto, TModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Update DTO -> Entity
        CreateMap<TNodeUpdateDto, TNode>()
            .IncludeBase<TModelUpdateDto, TModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}