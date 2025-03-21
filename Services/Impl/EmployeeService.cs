using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class EmployeeService
    : BaseService<Employee, EmployeeDTO, UpdateEmployeeDTO>,
        IEmployeeService
{
    public EmployeeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<EmployeeService> logger
    )
        : base(context, mapper, logger) { }

    public override async Task<EmployeeDTO?> UpdateAsync(int id, UpdateEmployeeDTO dto)
    {
        // ✅ Include EmployeeTeams to track relationship updates
        var entity = await _dbSet
            .Include(e => e.EmployeeTeams)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
        {
            _logger.LogWarning("Employee with ID {Id} not found.", id);
            return null;
        }

        _logger.LogInformation("Before Update: {Employee}", JsonSerializer.Serialize(entity));

        // ✅ Preserve existing MainID if not provided
        if (!string.IsNullOrEmpty(entity.MainID) && string.IsNullOrEmpty(dto.MainID))
        {
            dto.MainID = entity.MainID;
        }

        // ✅ Manual update
        dto.UpdateModel(entity);

        _logger.LogInformation("After Update: {Employee}", JsonSerializer.Serialize(entity));

        await _context.SaveChangesAsync();

        return _mapper.Map<EmployeeDTO>(entity);
    }
}
