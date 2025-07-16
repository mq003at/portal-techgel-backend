using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IMaterialService
    : IBaseService<Material, MaterialDTO, CreateMaterialDTO, UpdateMaterialDTO> { }
