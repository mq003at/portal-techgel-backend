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


    public override async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
    {
        // 1) Get the base list of DTOs (no RoleInfo lists yet)
        var dtos = (await base.GetAllAsync()).ToList();
        
        return dtos;
    }

    public override async Task<EmployeeDTO?> GetByIdAsync(int id)
    {
        var dto = await base.GetByIdAsync(id);


        return dto;
    }

    public override async Task<EmployeeDTO> CreateAsync(CreateEmployeeDTO dto)
    {
        _logger.LogInformation("CreateAsync called with DTO: {@dto}", dto);

        // employee generation
        var employee = _mapper.Map<Employee>(dto);
        _logger.LogError("PersonalInfo iD: {id}", employee.PersonalInfo.Id);
        _context.Employees.Add(employee);

        // manifest an id
        var mainId = "TG" + PadWithZeros(employee.Id);
        employee.MainId = mainId;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Employee inserted with Id={Id} and personalInfoId = {pId}", employee.Id, employee.PersonalInfo.Id);

        return _mapper.Map<EmployeeDTO>(employee);
    }

    public async Task<EmployeeDTO> UpdateEmployeeDetailsAsync(int employeeId, UpdateEmployeeDetailsDTO dto)
    {
        var employee = await _context.Employees
            .Include(e => e.CompanyInfo)
            .Include(e => e.ScheduleInfo)
            .Include(e => e.EmergencyContactInfos)
            .Include(e => e.EmployeeQualificationInfos)
            .Include(e => e.TaxInfo)
            .Include(e => e.CareerPathInfo)
            .Include(e => e.InsuranceInfo)
            .Include(e => e.PersonalInfo)
            .Include(e => e.OrganizationEntityEmployees)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

        if (dto.PersonalInfo != null)
        {
            // Update PersonalInfo
            if (employee.PersonalInfo == null)
            {
                var newPersonalInfo = _mapper.Map<PersonalInfo>(dto.PersonalInfo);
                newPersonalInfo.EmployeeId = employeeId;
                employee.PersonalInfo = newPersonalInfo;
                _context.PersonalInfos.Add(newPersonalInfo);
            }
            else
            {

                _mapper.Map(dto.PersonalInfo, employee.PersonalInfo);
                _context.Entry(employee.PersonalInfo).State = EntityState.Modified;
            }
        }

        // CompanyInfo
        if (dto.CompanyInfo != null)
        {

            if (employee.CompanyInfo == null)
            {
                var newCompanyInfo = _mapper.Map<CompanyInfo>(dto.CompanyInfo);
                newCompanyInfo.EmployeeId = employeeId;
                employee.CompanyInfo = newCompanyInfo;
                _context.CompanyInfos.Add(newCompanyInfo);
            }
            else
            {
                _mapper.Map(dto.CompanyInfo, employee.CompanyInfo);
                _context.Entry(employee.CompanyInfo).State = EntityState.Modified;
            }
        }

        // ScheduleInfo
        if (dto.ScheduleInfo != null)
        {
            if (employee.ScheduleInfo == null)
            {
                var newSchedule = _mapper.Map<ScheduleInfo>(dto.ScheduleInfo);
                newSchedule.EmployeeId = employeeId;
                employee.ScheduleInfo = newSchedule;
                _context.ScheduleInfos.Add(newSchedule);
            }
            else
            {
                _mapper.Map(dto.ScheduleInfo, employee.ScheduleInfo);
                _context.Entry(employee.ScheduleInfo).State = EntityState.Modified;
            }
        }

        // CareerPath
        if (dto.CareerPathInfo != null)
        {
            if (employee.CareerPathInfo == null)
            {
                var newCareer = _mapper.Map<CareerPathInfo>(dto.CareerPathInfo);
                newCareer.EmployeeId = employeeId;
                employee.CareerPathInfo = newCareer;
                _context.CareerPathInfos.Add(newCareer);
            }
            else
            {
                _mapper.Map(dto.CareerPathInfo, employee.CareerPathInfo);
                _context.Entry(employee.CareerPathInfo).State = EntityState.Modified;
            }
        }

        // TaxInfo
        if (dto.TaxInfo != null)
        {
            if (employee.TaxInfo == null)
            {
                var newTax = _mapper.Map<TaxInfo>(dto.TaxInfo);
                newTax.EmployeeId = employeeId;
                employee.TaxInfo = newTax;
                _context.TaxInfos.Add(newTax);
            }
            else
            {
                _mapper.Map(dto.TaxInfo, employee.TaxInfo);
                _context.Entry(employee.TaxInfo).State = EntityState.Modified;
            }
        }

        // InsuranceInfo
        if (dto.InsuranceInfo != null)
        {
            if (employee.InsuranceInfo == null)
            {
                var newInsurance = _mapper.Map<InsuranceInfo>(dto.InsuranceInfo);
                newInsurance.EmployeeId = employeeId;
                employee.InsuranceInfo = newInsurance;
                _context.InsuranceInfos.Add(newInsurance);
            }
            else
            {
                _context.Attach(employee.InsuranceInfo);
                _mapper.Map(dto.InsuranceInfo, employee.InsuranceInfo);
                _context.Entry(employee.InsuranceInfo).State = EntityState.Modified;
            }
        }

        if (dto.EmergencyContactInfos is { Count: > 0 })
        {
            // Step 1: Remove all existing contacts for this employee
            var existingContacts = await _context.EmergencyContactInfos
                .Where(c => c.EmployeeId == employeeId)
                .ToListAsync();

            _context.EmergencyContactInfos.RemoveRange(existingContacts);

            // Step 2: Add new contacts
            var newContacts = dto.EmergencyContactInfos.Select(c =>
            {
                var entity = _mapper.Map<EmergencyContactInfo>(c);
                entity.EmployeeId = employeeId;
                return entity;
            }).ToList();

            _context.EmergencyContactInfos.AddRange(newContacts);
        }

        // Qualifications (replace all)
        if (dto.EmployeeQualificationInfos is { Count: > 0 })
        {
            // Step 1: Remove existing qualifications
            var existingQualifications = await _context.EmployeeQualificationInfos
                .Where(q => q.EmployeeId == employeeId)
                .ToListAsync();

            _context.EmployeeQualificationInfos.RemoveRange(existingQualifications);

            // Step 2: Add new qualifications
            var newQualifications = dto.EmployeeQualificationInfos.Select(q =>
            {
                var entity = _mapper.Map<EmployeeQualificationInfo>(q);
                entity.EmployeeId = employeeId;
                return entity;
            }).ToList();

            _context.EmployeeQualificationInfos.AddRange(newQualifications);
        }

        // ---------- ROLE FUNCTIONALITIES ---------- //

        // Supervisor & DeputySupervisor
        if (dto.SupervisorId.HasValue)
            employee.SupervisorId = dto.SupervisorId;
        if (dto.DeputySupervisorId.HasValue)
            employee.DeputySupervisorId = dto.DeputySupervisorId;
        if (dto.DeputySubordinateIds != null)
            employee.DeputySubordinates = await _context.Employees.Where(e => dto.DeputySubordinateIds.Contains(e.Id)).ToListAsync();
        if (dto.SubordinateIds != null)
            employee.Subordinates = await _context.Employees.Where(e => dto.SubordinateIds.Contains(e.Id)).ToListAsync();

        // OrganizationEntityEmployees — clear and re-add
        if (dto.OrganizationEntityEmployees is { Count: > 0 })
        {
            var validUpdates = dto.OrganizationEntityEmployees
                .Where(x => x.OrganizationEntityId != 0 && x.OrganizationRelationType != 0)
                .Select(x => new OrganizationEntityEmployee
                {
                    EmployeeId = employeeId,
                    OrganizationEntityId = x.OrganizationEntityId,
                    OrganizationRelationType = x.OrganizationRelationType,
                    IsPrimary = x.IsPrimary
                }).ToList();

            // Remove old ones
            _context.OrganizationEntityEmployees.RemoveRange(employee.OrganizationEntityEmployees);
            employee.OrganizationEntityEmployees = validUpdates;
        }

        await _context.SaveChangesAsync();

        // Reload fully
        var updated = await _context.Employees
            .Include(e => e.CompanyInfo)
            .Include(e => e.ScheduleInfo)
            .Include(e => e.EmergencyContactInfos)
            .Include(e => e.EmployeeQualificationInfos)
            .Include(e => e.TaxInfo)
            .Include(e => e.CareerPathInfo)
            .Include(e => e.InsuranceInfo)
            .Include(e => e.PersonalInfo)
            .Include(e => e.Supervisor)
            .Include(e => e.DeputySupervisor)
            .Include(e => e.Subordinates)
            .Include(e => e.DeputySubordinates)
            .Include(e => e.OrganizationEntityEmployees).ThenInclude(o => o.OrganizationEntity)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        return _mapper.Map<EmployeeDTO>(updated!);
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
    
    private string PadWithZeros(int number, int totalLength = 5)
{
    return number.ToString().PadLeft(totalLength, '0');
}
}
