using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IBaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>
{
    Task<IEnumerable<TReadDTO>> GetAllAsync();
    Task<TReadDTO?> GetByIdAsync(int id);
    Task<TReadDTO> CreateAsync(TCreateDTO dto);
    Task<TReadDTO?> UpdateAsync(int id, TUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}

public interface IBaseModelWithOnlyIdService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>
{
    Task<IEnumerable<TReadDTO>> GetAllAsync();
    Task<TReadDTO?> GetByIdAsync(int id);
    Task<TReadDTO> CreateAsync(TCreateDTO dto);
    Task<TReadDTO?> UpdateAsync(int id, TUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}
