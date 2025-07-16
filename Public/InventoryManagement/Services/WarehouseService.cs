using AutoMapper;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class WarehouseService
    : BaseService<Warehouse, WarehouseDTO, CreateWarehouseDTO, UpdateWarehouseDTO>,
        IWarehouseService
{
    public WarehouseService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<WarehouseService> logger
    )
        : base(context, mapper, logger) { }
}
