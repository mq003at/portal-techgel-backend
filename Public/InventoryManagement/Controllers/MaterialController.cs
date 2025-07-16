namespace portal.Controllers;

using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

[Route("api/[controller]s")]
[ApiController]
public class MaterialController
    : BaseController<Material, MaterialDTO, CreateMaterialDTO, UpdateMaterialDTO>
{
    public MaterialController(IMaterialService service)
        : base(service) { }
}
