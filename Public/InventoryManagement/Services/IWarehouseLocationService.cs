using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IWarehouseLocationService
    : IBaseService<
        WarehouseLocation,
        WarehouseLocationDTO,
        WarehouseLocationCreateDTO,
        WarehouseLocationUpdateDTO
    > { }
