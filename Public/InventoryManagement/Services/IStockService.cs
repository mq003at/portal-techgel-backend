using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IStockService : IBaseService<Stock, StockDTO, StockCreateDTO, StockUpdateDTO>
{
    Task<IEnumerable<StockDTO>> GetByMaterialAsync(int materialId);
}
