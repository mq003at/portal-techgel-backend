using portal.DTOs;
using portal.Models;

namespace portal.Mappings;

public abstract class BaseWorkflowProfile<
    TWorkflow,
    TWorkflowDto,
    TWorkflowCreateDto,
    TWorkflowUpdateDto,
    TModel,             // <- BaseModel
    TModelDto,
    TModelCreateDto,
    TModelUpdateDto
> : BaseModelProfile<TModel, TModelDto, TModelCreateDto, TModelUpdateDto>
    where TWorkflow : BaseWorkflow
    where TWorkflowDto : BaseWorkflowDTO
    where TWorkflowCreateDto : BaseModelCreateDTO
    where TWorkflowUpdateDto : BaseModelUpdateDTO
    where TModel : BaseModel
    where TModelDto : BaseModelDTO
    where TModelCreateDto : BaseModelCreateDTO
    where TModelUpdateDto : BaseModelUpdateDTO
{
    public BaseWorkflowProfile()
    {
        // Entity <-> DTO (for main DTOs, including navigations)
        CreateMap<TWorkflow, TWorkflowDto>()
            .IncludeBase<TModel, TModelDto>()
                .ReverseMap();

        // Create DTO -> Entity (client -> server)
        CreateMap<TWorkflowCreateDto, TWorkflow>()
            .IncludeBase<TModelCreateDto, TModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            ;

        // Update DTO -> Entity (client -> server)
        CreateMap<TWorkflowUpdateDto, TWorkflow>()
            .IncludeBase<TModelUpdateDto, TModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            ;
    }
}