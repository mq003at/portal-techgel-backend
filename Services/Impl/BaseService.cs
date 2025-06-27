// portal/Services/BaseService.cs
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class BaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>
    : IBaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>
    where TModel : BaseModel, new()
    where TReadDTO : BaseModelDTO
    where TCreateDTO : BaseModelCreateDTO
    where TUpdateDTO : BaseModelUpdateDTO
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TModel> _dbSet;
    protected readonly IMapper _mapper;
    protected readonly ILogger _logger;

    public BaseService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<BaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>> logger
    )
    {
        _context = context;
        _dbSet = _context.Set<TModel>();
        _mapper = mapper;
        _logger = logger;
    }

    public virtual async Task<IEnumerable<TReadDTO>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
        return _mapper.Map<IEnumerable<TReadDTO>>(entities);
    }

    public virtual async Task<TReadDTO?> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity == null ? null : _mapper.Map<TReadDTO>(entity);
    }

    public virtual async Task<TReadDTO> CreateAsync(TCreateDTO dto)
    {
        _logger.LogInformation($"Creating: {JsonSerializer.Serialize(dto)}");
        var entity = _mapper.Map<TModel>(dto); // Map hết DTO → Entity
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<TReadDTO>(entity);
    }

    public virtual async Task<TReadDTO?> UpdateAsync(int id, TUpdateDTO dto)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return null;

        _logger.LogInformation($"Updating before: {JsonSerializer.Serialize(entity)}");

        entity.MainId = "";

        // Áp DTO lên entity
        _mapper.Map(dto, entity);

        await _context.SaveChangesAsync();
        return _mapper.Map<TReadDTO>(entity);
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

public class BaseModelWithOnlyIdService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>
    : IBaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>
    where TModel : BaseModelWithOnlyId, new()
    where TReadDTO : BaseModelWithOnlyIdDTO
    where TCreateDTO : BaseModelWithOnlyIdCreateDTO
    where TUpdateDTO : BaseModelWithOnlyIdUpdateDTO
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TModel> _dbSet;
    protected readonly IMapper _mapper;
    protected readonly ILogger _logger;

    public BaseModelWithOnlyIdService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<BaseModelWithOnlyIdService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>> logger
    )
    {
        _context = context;
        _dbSet = _context.Set<TModel>();
        _mapper = mapper;
        _logger = logger;
    }

    public virtual async Task<IEnumerable<TReadDTO>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
        return _mapper.Map<IEnumerable<TReadDTO>>(entities);
    }

    public virtual async Task<TReadDTO?> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity == null ? null : _mapper.Map<TReadDTO>(entity);
    }

    public virtual async Task<TReadDTO> CreateAsync(TCreateDTO dto)
    {
        _logger.LogInformation($"Creating: {JsonSerializer.Serialize(dto)}");
        var entity = _mapper.Map<TModel>(dto); // Map hết DTO → Entity
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<TReadDTO>(entity);
    }

    public virtual async Task<TReadDTO?> UpdateAsync(int id, TUpdateDTO dto)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return null;

        _logger.LogInformation($"Updating before: {JsonSerializer.Serialize(entity)}");
        // Áp DTO lên entity
        _mapper.Map(dto, entity);

        await _context.SaveChangesAsync();
        return _mapper.Map<TReadDTO>(entity);
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