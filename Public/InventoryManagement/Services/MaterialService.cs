using AutoMapper;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class MaterialService
    : BaseService<Material, MaterialDTO, CreateMaterialDTO, UpdateMaterialDTO>,
        IMaterialService
{
    public MaterialService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<MaterialService> logger
    )
        : base(context, mapper, logger) { }
}
