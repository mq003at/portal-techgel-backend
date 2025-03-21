namespace portal.Services;

using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class BaseService<TModel, TDTO, TUpdateDTO> : IBaseService<TModel, TDTO, TUpdateDTO>
    where TModel : BaseModel, new()
    where TDTO : BaseDTO<TModel>, new()
    where TUpdateDTO : BaseDTO<TModel>
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TModel> _dbSet;
    protected readonly IMapper _mapper;
    protected readonly ILogger<BaseService<TModel, TDTO, TUpdateDTO>> _logger;

    public BaseService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<BaseService<TModel, TDTO, TUpdateDTO>> logger
    )
    {
        _context = context;
        _dbSet = _context.Set<TModel>();
        _mapper = mapper;
        _logger = logger;
    }

    public virtual async Task<IEnumerable<TDTO>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
        return _mapper.Map<IEnumerable<TDTO>>(entities);
    }

    public virtual async Task<TDTO?> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity == null ? null : _mapper.Map<TDTO>(entity);
    }

    public virtual async Task<TDTO> CreateAsync(TDTO dto)
    {
        _logger.LogInformation($"Before Create: {JsonSerializer.Serialize(dto)}");

        var entity = _mapper.Map<TModel>(dto);

        dto.UpdateModel(entity);
        _logger.LogInformation($"Entity: {JsonSerializer.Serialize(entity)}");

        _dbSet.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<TDTO>(entity);
    }

    public virtual async Task<TDTO?> UpdateAsync(int id, TUpdateDTO dto)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return null;

        _logger.LogInformation($"Before Update: {JsonSerializer.Serialize(entity)}");

        // Preserve MainID
        if (!string.IsNullOrEmpty(entity.MainID) && string.IsNullOrEmpty(dto.MainID))
        {
            dto.MainID = entity.MainID;
        }

        // Use manual model update logic
        dto.UpdateModel(entity);

        _logger.LogInformation($"After Update: {JsonSerializer.Serialize(entity)}");

        await _context.SaveChangesAsync();
        return _mapper.Map<TDTO>(entity);
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
