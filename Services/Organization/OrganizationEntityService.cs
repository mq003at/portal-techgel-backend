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

    // public override async Task<bool> CreateAsync(CreateOrganizationEntityDTO dto)
    // {
    //     _logger.LogInformation("Creating OrganizationEntity with employees: {@DTO}", dto);

    //     var entity = _mapper.Map<OrganizationEntity>(dto);
    //     _dbSet.Add(entity);
    //     await _context.SaveChangesAsync(); // To get the generated Id
    //     return true;
    // }

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
    public override async Task<OrganizationEntityDTO?> UpdateAsync(int id, UpdateOrganizationEntityDTO dto)
    {
        return await base.UpdateAsync(id, dto);
    }

    public async Task<bool> UpdateEmployeesAsync(
        int id,
        List<CreateOrganizationEntityEmployeeDTO> dtos
    )
    {
        _logger.LogInformation("Updating employees for OrganizationEntity: {Id}", id);
        // remove all links in OrganizationEntityEmployees that has OrganizationEntityId == id
        var existingLinks = await _context.OrganizationEntityEmployees
            .Where(link => link.OrganizationEntityId == id)
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
                IsPrimary = dto.IsPrimary,
                OrganizationRelationType = dto.OrganizationRelationType != default ? dto.OrganizationRelationType : OrganizationRelationType.MEMBER
                
            };
            _context.OrganizationEntityEmployees.Add(link);
        }
        await _context.SaveChangesAsync();
        return true;
    }
}

