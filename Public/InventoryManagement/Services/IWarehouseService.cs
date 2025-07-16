using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IWarehouseService
    : IBaseService<Warehouse, WarehouseDTO, CreateWarehouseDTO, UpdateWarehouseDTO> { }
