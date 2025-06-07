using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class EmployeeService
    : BaseService<Employee, EmployeeDTO, CreateEmployeeDTO, UpdateEmployeeDTO>,
        IEmployeeService
{
    private readonly DbSet<Employee> _employees;
    private readonly DbSet<OrganizationEntity> _orgEntities;
    private readonly DbSet<OrganizationEntityEmployee> _oee;

    public EmployeeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<EmployeeService> logger
    )
        : base(context, mapper, logger)
    {
        _employees = context.Set<Employee>();
        _orgEntities = context.Set<OrganizationEntity>();
        _oee = context.Set<OrganizationEntityEmployee>();
    }

    private async Task PopulateRoleInfoAsync(EmployeeDTO dto)
    {
        var id = dto.Id;

        // 1) Load M–M links and entity names
        var links = await _oee.Where(o => o.EmployeeId == id)
            .Include(o => o.OrganizationEntity)
            .ToListAsync();

        dto.RoleInfo.OrganizationEntityIds = links.Select(o => o.OrganizationEntityId).ToList();

        dto.RoleInfo.OrganizationEntityNames = links
            .Select(o => o.OrganizationEntity.Name)
            .ToList();

        // 2) Load subordinates by SupervisorId
        var subs = await _employees.Where(e => e.RoleInfo.SupervisorId == id).ToListAsync();

        dto.RoleInfo.SubordinateIds = subs.Select(e => e.Id).ToList();

        dto.RoleInfo.SubordinateNames = subs.Select(e => e.LastName + " " + e.MiddleName + " " + e.LastName).ToList();
    }

    public override async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
    {
        // 1) Get the base list of DTOs (no RoleInfo lists yet)
        var dtos = (await base.GetAllAsync()).ToList();

        // 2) For each DTO, populate the RoleInfo join-table and subordinate data
        foreach (var dto in dtos)
        {
            await PopulateRoleInfoAsync(dto);
        }

        return dtos;
    }

    public override async Task<EmployeeDTO?> GetByIdAsync(int id)
    {
        var dto = await base.GetByIdAsync(id);

        if (dto == null)
        {
            throw new KeyNotFoundException($"Employee {id} not found");
        }

        await PopulateRoleInfoAsync(dto);
        return dto;
    }

    public override async Task<EmployeeDTO> CreateAsync(CreateEmployeeDTO dto)
    {
        _logger.LogInformation("CreateAsync called with DTO: {@dto}", dto);

        // 1) Map
        var entity = _mapper.Map<Employee>(dto);
        _logger.LogDebug("Mapped entity (pre‐save): {@entity}", entity);
        _logger.LogDebug(
            "Timestamps before save: CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt}",
            entity.CreatedAt,
            entity.UpdatedAt
        );

        // 2) Validation checks
        if (
            entity.RoleInfo.SupervisorId != 0
            && !await _employees.AnyAsync(e => e.Id == entity.RoleInfo.SupervisorId)
        )
        {
            _logger.LogWarning(
                "Invalid SupervisorId: {SupervisorId}",
                entity.RoleInfo.SupervisorId
            );
            throw new ArgumentException("Invalid SupervisorId");
        }

        // ... other subordinate/org checks, with similar _logger.LogWarning if invalid ...

        // 3) Add & Save
        _employees.Add(entity);

        await _context.SaveChangesAsync();

        entity.MainId = GenerateMainIdForEmployee(
            "TG",
            entity.Id
        );

        _context.Employees.Update(entity);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Employee inserted with Id={Id}", entity.Id);

        // 4) Link OrganizationEntityEmployees if any
        if (dto.RoleInfo?.OrganizationEntityIds != null)
        {
            foreach (var oeId in dto.RoleInfo.OrganizationEntityIds)
            {
                _oee.Add(
                    new OrganizationEntityEmployee
                    {
                        EmployeeId = entity.Id,
                        OrganizationEntityId = oeId
                    }
                );
            }
            await _context.SaveChangesAsync();
        }

        // 5) Return fresh DTO
        var result = await GetByIdAsync(entity.Id);
        if (result == null)
        {
            _logger.LogError("Failed to retrieve EmployeeDTO after save for Id={Id}", entity.Id);
            throw new InvalidOperationException("Failed to retrieve the updated Employee.");
        }

        _logger.LogInformation("CreateAsync completed successfully for Id={Id}", entity.Id);
        return result;
    }

    public override async Task<EmployeeDTO?> UpdateAsync(int id, UpdateEmployeeDTO dto)
    {
        // 1) load the employee (owned RoleInfo will come down automatically)
        var entity =
            await _employees.FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new KeyNotFoundException("Employee not found");

        // 2) load and cache the existing link rows
        var existingLinks = await _oee.Where(o => o.EmployeeId == id).ToListAsync();

        // … your validation …

        // 3) map DTO → entity
        _mapper.Map(dto, entity);

        // 4) replace the links
        if (dto.RoleInfo?.OrganizationEntityIds != null)
        {
            _oee.RemoveRange(existingLinks);
            foreach (var oeId in dto.RoleInfo.OrganizationEntityIds)
            {
                _oee.Add(
                    new OrganizationEntityEmployee
                    {
                        EmployeeId = entity.Id,
                        OrganizationEntityId = oeId
                    }
                );
            }
        }

        // 5) save
        _employees.Update(entity);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(entity.Id);
    }

    public async Task<EmployeeDTO> LoginAsync(string MainId, string password)
    {
        var employee =
            await _employees.FirstOrDefaultAsync(e => e.MainId == MainId && e.Password == password)
            ?? throw new UnauthorizedAccessException("Invalid credentials");

        return _mapper.Map<EmployeeDTO>(employee);
    }

    public async Task<List<string>> GetUserNamesByIdsAsync(List<int> userIds)
    {
        // Replace Employee with your actual user entity name if needed
        var employees = await _context.Set<Employee>()
            .Where(e => userIds.Contains(e.Id))
            .ToListAsync();

        // You can adjust formatting as needed
        return employees
            .OrderBy(e => userIds.IndexOf(e.Id)) // maintain original order
            .Select(e => $"{e.FirstName} {e.LastName}".Trim())
            .ToList();
    }

    public static string GenerateMainIdForEmployee(string prefix, int number, int totalDigits = 5)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("Prefix cannot be empty.");

        if (number < 0)
            throw new ArgumentException("Number must be non-negative.");

        return $"{prefix}{number.ToString().PadLeft(totalDigits, '0')}";
    }
}
