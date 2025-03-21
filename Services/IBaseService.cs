using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IBaseService<TModel, TDTO, TUpdateDTO>
{
    Task<IEnumerable<TDTO>> GetAllAsync();
    Task<TDTO?> GetByIdAsync(int id);
    Task<TDTO> CreateAsync(TDTO dto);
    Task<TDTO?> UpdateAsync(int id, TUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}
