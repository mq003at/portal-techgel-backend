namespace portal.Controllers;

using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

[Route("api/[controller]s")]
[ApiController]
public class WarehouseLocationController
    : BaseController<
        WarehouseLocation,
        WarehouseLocationDTO,
        WarehouseLocationCreateDTO,
        WarehouseLocationUpdateDTO
    >
{
    public WarehouseLocationController(IWarehouseLocationService service)
        : base(service) { }
}
