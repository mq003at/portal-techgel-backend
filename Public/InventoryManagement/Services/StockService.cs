using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class StockService
    : BaseService<Stock, StockDTO, StockCreateDTO, StockUpdateDTO>,
        IStockService
{
    public StockService(ApplicationDbContext context, IMapper mapper, ILogger<StockService> logger)
        : base(context, mapper, logger) { }

    public override async Task<StockDTO> CreateAsync(StockCreateDTO dto)
    {
        _logger.LogInformation("Creating stock: {Dto}", JsonSerializer.Serialize(dto));

        var stock = _mapper.Map<Stock>(dto);
        stock.StockLocations = dto
            .Locations.Select(loc => new StockLocation
            {
                WarehouseLocationId = loc.WarehouseLocationId,
                Quantity = loc.Quantity
            })
            .ToList();

        _dbSet.Add(stock);
        await _context.SaveChangesAsync();

        return _mapper.Map<StockDTO>(stock);
    }

    public override async Task<StockDTO?> UpdateAsync(int id, StockUpdateDTO dto)
    {
        var stock = await _dbSet
            .Include(s => s.StockLocations)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stock == null)
            return null;

        _mapper.Map(dto, stock); // materialId, warehouseId, etc.

        if (dto.Locations is not null)
        {
            stock.StockLocations.Clear();

            foreach (var loc in dto.Locations)
            {
                stock.StockLocations.Add(
                    new StockLocation
                    {
                        WarehouseLocationId = loc.WarehouseLocationId,
                        Quantity = loc.Quantity
                    }
                );
            }
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<StockDTO>(stock);
    }

    public async Task<IEnumerable<StockDTO>> GetByMaterialAsync(int materialId)
    {
        var stocks = await _dbSet
            .Include(s => s.Material)
            .Include(s => s.Warehouse)
            .Include(s => s.StockLocations)
            .ThenInclude(sl => sl.WarehouseLocation)
            .Where(s => s.MaterialId == materialId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<StockDTO>>(stocks);
    }
}
