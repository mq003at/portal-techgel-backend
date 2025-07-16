namespace portal.Controllers;

using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

[Route("api/[controller]s")]
[ApiController]
public class WarehouseController
    : BaseController<Warehouse, WarehouseDTO, CreateWarehouseDTO, UpdateWarehouseDTO>
{
    public WarehouseController(IWarehouseService service)
        : base(service) { }
}
