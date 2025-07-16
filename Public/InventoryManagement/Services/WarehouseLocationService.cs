using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class WarehouseLocationService
    : BaseService<
        WarehouseLocation,
        WarehouseLocationDTO,
        WarehouseLocationCreateDTO,
        WarehouseLocationUpdateDTO
    >,
        IWarehouseLocationService
{
    public WarehouseLocationService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<WarehouseLocationService> logger
    )
        : base(context, mapper, logger) { }
}
