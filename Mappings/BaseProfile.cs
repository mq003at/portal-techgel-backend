using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;

public abstract class BaseModelProfile<
    TModel,
    TDto,
    TCreateDto,
    TUpdateDto>
    : Profile
    where TModel : BaseModel
    where TDto : BaseModelDTO
    where TCreateDto : BaseModelCreateDTO
    where TUpdateDto : BaseModelUpdateDTO
{
    public BaseModelProfile()
    {
        // Model -> DTO
        CreateMap<TModel, TDto>().ReverseMap();
        // Create DTO -> Model
        CreateMap<TCreateDto, TModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Update DTO -> Model
        CreateMap<TUpdateDto, TModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}