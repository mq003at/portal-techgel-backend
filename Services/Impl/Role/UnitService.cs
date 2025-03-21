namespace portal.Services;

using AutoMapper;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class UnitService : BaseService<Unit, UnitDTO, UpdateUnitDTO>, IUnitService
{
    public UnitService(ApplicationDbContext context, IMapper mapper, ILogger<UnitService> logger)
        : base(context, mapper, logger) { }
}
