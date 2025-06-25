using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;

namespace portal.Services;

public class OrganizationEntityService
    : BaseService<
        OrganizationEntity,
        OrganizationEntityDTO,
        CreateOrganizationEntityDTO,
        UpdateOrganizationEntityDTO
    >,
        IOrganizationEntityService
{
    public OrganizationEntityService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<OrganizationEntityService> logger
    )
        : base(context, mapper, logger) { }

    public override async Task<IEnumerable<OrganizationEntityDTO>> GetAllAsync()
    {
        var entities = await _dbSet
            .Include(o => o.Parent)
            .Include(o => o.Children)
            .Include(o => o.OrganizationEntityEmployees)
                .ThenInclude(link => link.Employee)
            .ToListAsync();

        return _mapper.Map<IEnumerable<OrganizationEntityDTO>>(entities);
    }

    public override async Task<OrganizationEntityDTO> CreateAsync(CreateOrganizationEntityDTO dto)
    {
        _logger.LogInformation("Creating OrganizationEntity with employees: {@DTO}", dto);

        // 1. Map the core entity
        var entity = _mapper.Map<OrganizationEntity>(dto);
        _dbSet.Add(entity);
        await _context.SaveChangesAsync(); // To get the generated Id

        // 2. Add employee links if provided
        if (dto.EmployeeIds != null && dto.EmployeeIds.Count > 0)
        {
            foreach (var empId in dto.EmployeeIds)
            {
                var relationType = OrganizationRelationType.MEMBER;

                if (dto.ManagerId.HasValue && empId == dto.ManagerId.Value)
                    relationType = OrganizationRelationType.MANAGER;
                else if (dto.DeputyManagerId.HasValue && empId == dto.DeputyManagerId.Value)
                    relationType = OrganizationRelationType.DEPUTY_MANAGER;

                _context.OrganizationEntityEmployees.Add(new OrganizationEntityEmployee
                {
                    OrganizationEntityId = entity.Id,
                    EmployeeId = empId,
                    OrganizationRelationType = relationType,
                    IsPrimary = true
                });
            }

            await _context.SaveChangesAsync();
        }
        else
        {
            // If ManagerId or DeputyManagerId are set but not included in EmployeeIds, add them explicitly
            if (dto.ManagerId.HasValue)
            {
                _context.OrganizationEntityEmployees.Add(new OrganizationEntityEmployee
                {
                    OrganizationEntityId = entity.Id,
                    EmployeeId = dto.ManagerId.Value,
                    OrganizationRelationType = OrganizationRelationType.MANAGER,
                    IsPrimary = true
                });
            }

            if (dto.DeputyManagerId.HasValue)
            {
                _context.OrganizationEntityEmployees.Add(new OrganizationEntityEmployee
                {
                    OrganizationEntityId = entity.Id,
                    EmployeeId = dto.DeputyManagerId.Value,
                    OrganizationRelationType = OrganizationRelationType.DEPUTY_MANAGER,
                    IsPrimary = true
                });
            }

            await _context.SaveChangesAsync();
        }

        // 3. Reload full entity with navigation for DTO mapping
        var result = await _dbSet
            .Include(o => o.Parent)
            .Include(o => o.Children)
            .Include(o => o.OrganizationEntityEmployees)
                .ThenInclude(link => link.Employee)
            .FirstOrDefaultAsync(o => o.Id == entity.Id);

        return _mapper.Map<OrganizationEntityDTO>(result!);
    }
    public override async Task<OrganizationEntityDTO?> GetByIdAsync(int id)
    {
        var entity = await _dbSet
            .Include(o => o.Parent)
            .Include(o => o.Children)
            .Include(o => o.OrganizationEntityEmployees)
                .ThenInclude(link => link.Employee)
            .FirstOrDefaultAsync(o => o.Id == id);

        return entity == null ? null : _mapper.Map<OrganizationEntityDTO>(entity);
    }

    public override async Task<OrganizationEntityDTO?> UpdateAsync(int id, UpdateOrganizationEntityDTO dto)
    {
        var entity = await _dbSet
            .Include(e => e.OrganizationEntityEmployees)
                .ThenInclude(link => link.Employee)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
            return null;

        _logger.LogInformation("Updating OrganizationEntity: {Id}", id);

        // 1. Update scalar properties
        _mapper.Map(dto, entity);

        // 2. Sync EmployeeIds (many-to-many links)
        if (dto.EmployeeIds != null)
        {
            var existingEmployeeIds = entity.OrganizationEntityEmployees.Select(e => e.EmployeeId).ToList();
            var newEmployeeIds = dto.EmployeeIds;

            var toAdd = newEmployeeIds.Except(existingEmployeeIds).ToList();
            var toRemove = existingEmployeeIds.Except(newEmployeeIds).ToList();

            entity.OrganizationEntityEmployees.RemoveAll(e => toRemove.Contains(e.EmployeeId));

            foreach (var empId in toAdd)
            {
                entity.OrganizationEntityEmployees.Add(new OrganizationEntityEmployee
                {
                    OrganizationEntityId = entity.Id,
                    EmployeeId = empId,
                    OrganizationRelationType = OrganizationRelationType.MEMBER,
                    IsPrimary = dto.IsPrimary ?? false
                });
            }
        }

        // 3. Update manager relation
        if (dto.MangerId.HasValue)
        {
            // Demote existing manager
            foreach (var e in entity.OrganizationEntityEmployees
                         .Where(e => e.OrganizationRelationType == OrganizationRelationType.MANAGER))
            {
                e.OrganizationRelationType = OrganizationRelationType.MEMBER;
            }

            // Promote new manager
            var existing = entity.OrganizationEntityEmployees
                .FirstOrDefault(e => e.EmployeeId == dto.MangerId.Value);
            if (existing != null)
            {
                existing.OrganizationRelationType = OrganizationRelationType.MANAGER;
            }
            else
            {
                entity.OrganizationEntityEmployees.Add(new OrganizationEntityEmployee
                {
                    OrganizationEntityId = entity.Id,
                    EmployeeId = dto.MangerId.Value,
                    OrganizationRelationType = OrganizationRelationType.MANAGER,
                    IsPrimary = true
                });
            }
        }

        // 4. Update deputy manager relation
        if (dto.DeputyManagerId.HasValue)
        {
            foreach (var e in entity.OrganizationEntityEmployees
                         .Where(e => e.OrganizationRelationType == OrganizationRelationType.DEPUTY_MANAGER))
            {
                e.OrganizationRelationType = OrganizationRelationType.MEMBER;
            }

            var existing = entity.OrganizationEntityEmployees
                .FirstOrDefault(e => e.EmployeeId == dto.DeputyManagerId.Value);
            if (existing != null)
            {
                existing.OrganizationRelationType = OrganizationRelationType.DEPUTY_MANAGER;
            }
            else
            {
                entity.OrganizationEntityEmployees.Add(new OrganizationEntityEmployee
                {
                    OrganizationEntityId = entity.Id,
                    EmployeeId = dto.DeputyManagerId.Value,
                    OrganizationRelationType = OrganizationRelationType.DEPUTY_MANAGER,
                    IsPrimary = true
                });
            }
        }

        await _context.SaveChangesAsync();

        // Reload full entity for DTO mapping
        var updated = await _dbSet
            .Include(o => o.Parent)
            .Include(o => o.Children)
            .Include(o => o.OrganizationEntityEmployees)
                .ThenInclude(oe => oe.Employee)
            .FirstOrDefaultAsync(o => o.Id == entity.Id);

        return _mapper.Map<OrganizationEntityDTO>(updated!);
    }
}
