namespace portal.Controllers;

using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

[Route("api/[controller]s")]
[ApiController]
public class StockController : BaseController<Stock, StockDTO, StockCreateDTO, StockUpdateDTO>
{
    public StockController(IStockService service)
        : base(service) { }

    [HttpGet("material/{materialId}")]
    public async Task<IActionResult> GetByMaterial(int materialId)
    {
        var stocks = await _service.GetByMaterialAsync(materialId);

        return Ok(stocks);
    }
}
