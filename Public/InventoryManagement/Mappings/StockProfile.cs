namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Models;

public class StockProfile : BaseModelProfile<Stock, StockDTO, StockCreateDTO, StockUpdateDTO>
{
    public StockProfile()
    {
        // Stock → StockDTO
        CreateMap<Stock, StockDTO>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.Name))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
            .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.StockLocations));

        // StockLocation → StockLocationDTO
        CreateMap<StockLocation, StockLocationDTO>()
            .ForMember(
                dest => dest.LocationCode,
                opt => opt.MapFrom(src => src.WarehouseLocation.Code)
            );

        // StockCreateDTO → Stock (flattened)
        CreateMap<StockCreateDTO, Stock>()
            .IncludeBase<BaseModelCreateDTO, BaseModel>();

        CreateMap<StockUpdateDTO, Stock>().IncludeBase<BaseModelUpdateDTO, BaseModel>();

        // StockLocationCreateDTO → StockLocation
        CreateMap<StockLocationCreateDTO, StockLocation>();
    }
}
