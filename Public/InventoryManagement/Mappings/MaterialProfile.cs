namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Models;

public class MaterialProfile
    : BaseModelProfile<Material, MaterialDTO, CreateMaterialDTO, UpdateMaterialDTO>
{
    public MaterialProfile()
    {
        // Map model → DTO
        CreateMap<Material, MaterialDTO>();

        // Map create/update DTOs → model
        CreateMap<CreateMaterialDTO, Material>()
            .IncludeBase<BaseModelCreateDTO, BaseModel>();

        CreateMap<UpdateMaterialDTO, Material>().IncludeBase<BaseModelUpdateDTO, BaseModel>();
    }
}
