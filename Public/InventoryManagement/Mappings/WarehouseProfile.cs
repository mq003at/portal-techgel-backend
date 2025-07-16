using portal.DTOs;
using portal.Extensions;
using portal.Models;

namespace portal.Mappings;

public class WarehouseProfile
    : BaseModelProfile<Warehouse, WarehouseDTO, CreateWarehouseDTO, UpdateWarehouseDTO>
{
    public WarehouseProfile()
    {
        // Append additional mapping on top of base
        CreateMap<Warehouse, WarehouseDTO>()
            .IncludeBase<BaseModel, BaseModelDTO>()
            .ForMember(
                dest => dest.ManagerName,
                opt => opt.MapFrom(src => src.Manager.GetDisplayName())
            ); // assuming navigation property

        CreateMap<CreateWarehouseDTO, Warehouse>().IncludeBase<BaseModelCreateDTO, BaseModel>();

        CreateMap<UpdateWarehouseDTO, Warehouse>().IncludeBase<BaseModelUpdateDTO, BaseModel>();
    }
}
