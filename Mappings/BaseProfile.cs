using AutoMapper;

namespace portal.Mappings;

public abstract class BaseModelProfile<TModel, TDto> : Profile
{
    public BaseModelProfile()
    {
        // Example mapping: BaseModel to itself (customize as needed)
        CreateMap<TModel, TDto>(MemberList.Source);
        CreateMap<TDto, TModel>(MemberList.Destination);
    }
}
