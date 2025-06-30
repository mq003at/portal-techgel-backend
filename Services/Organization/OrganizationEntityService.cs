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
        var entity = _mapper.Map<OrganizationEntity>(dto);

        

        if (entity.ManagerId != null)
        {
            int managerId = entity.ManagerId.Value;
            Employee manager =
                await _context.Employees.FindAsync(managerId)
                ?? throw new ArgumentException($"Không tìm thấy nhân viên với Id {managerId}.");
            entity.Manager = manager;

            _context.OrganizationEntityEmployees.Add(
                new OrganizationEntityEmployee
                {
                    OrganizationEntityId = entity.Id,
                    OrganizationEntity = entity,
                    EmployeeId = managerId,
                    Employee = manager
                }
            );
        }

        if (entity.DeputyManagerId != null)
        {
            int deputyManagerId = entity.DeputyManagerId.Value;
            Employee deputyManager =
                await _context.Employees.FindAsync(deputyManagerId)
                ?? throw new ArgumentException(
                    $"Không tìm thấy nhân viên với Id {deputyManagerId}."
                );
            entity.DeputyManager = deputyManager;

            _context.OrganizationEntityEmployees.Add(
                new OrganizationEntityEmployee
                {
                    OrganizationEntityId = entity.Id,
                    OrganizationEntity = entity,
                    EmployeeId = deputyManagerId,
                    Employee = deputyManager
                }
            );
        }
        _dbSet.Add(entity);
        await _context.SaveChangesAsync(); // To get the generated Id
        if (entity.ParentId != null)
        {
            OrganizationEntity parent =
                await _dbSet
                    .Include(p => p.Children)
                    .FirstOrDefaultAsync(p => p.Id == entity.ParentId)
                ?? throw new ArgumentException($"Không tìm thấy tổ chức cấp trên.");
            ;
            // Add to list parents Ids
            entity.Parent = parent;
            if (parent.Children == null)
            {
                parent.Children = new List<OrganizationEntity>();
            }
            parent.Children.Add(entity);
            parent.ChildrenIds.Add(entity.Id);
            _context.OrganizationEntities.Update(parent);
        }
        await _context.SaveChangesAsync();
        return _mapper.Map<OrganizationEntityDTO>(entity);
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

    // only update metadata of the Org
    public override async Task<OrganizationEntityDTO?> UpdateAsync(
        int id,
        UpdateOrganizationEntityDTO dto
    )
    {
        return await base.UpdateAsync(id, dto);
    }

    public async Task<bool> UpdateEmployeesAsync(
        int id,
        List<OrganizationEntityEmployeeCreateDTO> dtos
    )
    {
        _logger.LogInformation("Updating employees for OrganizationEntity: {Id}", id);
        // remove all links in OrganizationEntityEmployees that has OrganizationEntityId == id
        var existingLinks = await _context
            .OrganizationEntityEmployees.Where(link => link.OrganizationEntityId == id)
            .ToListAsync();
        if (existingLinks.Any())
        {
            _context.OrganizationEntityEmployees.RemoveRange(existingLinks);
            await _context.SaveChangesAsync();
        }
        // add new links
        foreach (var dto in dtos)
        {
            var link = new OrganizationEntityEmployee
            {
                OrganizationEntityId = id,
                EmployeeId = dto.EmployeeId,
            };
            _context.OrganizationEntityEmployees.Add(link);
        }
        await _context.SaveChangesAsync();
        return true;
    }
}
