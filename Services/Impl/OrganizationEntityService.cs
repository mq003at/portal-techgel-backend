using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class OrganizationEntityService
    : BaseService<
        OrganizationEntity,
        OrganizationEntitySummaryDTO,
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

    public override async Task<OrganizationEntitySummaryDTO> CreateAsync(
        CreateOrganizationEntityDTO dto
    )
    {
        //  Map các field cơ bản
        var entity = _mapper.Map<OrganizationEntity>(dto);

        //  Nếu có ParentId, kiểm tra tồn tại
        if (dto.ParentId.HasValue)
        {
            var parent = await _context.OrganizationEntities.FindAsync(dto.ParentId.Value);
            if (parent == null)
                throw new InvalidOperationException("Entity này không tồn tại.");
            entity.Parent = parent;
        }

        //  Nếu có childrenIds, lấy ra tất cả và kiểm tra
        if (dto.ChildrenIds != null && dto.ChildrenIds.Any())
        {
            var children = await _context
                .OrganizationEntities.Where(o => dto.ChildrenIds.Contains(o.Id))
                .ToListAsync();
            if (children.Count != dto.ChildrenIds.Count)
                throw new InvalidOperationException("Entity này không tồn tại.");
            entity.Children = children;
        }

        // Xử lý EmployeeIds
        if (dto.EmployeeIds != null && dto.EmployeeIds.Any())
        {
            // Lấy tất cả nhân viên hợp lệ
            var employees = await _context
                .Employees.Where(e => dto.EmployeeIds.Contains(e.Id))
                .ToListAsync();

            if (employees.Count != dto.EmployeeIds.Count)
                throw new KeyNotFoundException("Một hoặc nhiều employeeId không tồn tại.");

            // Tạo liên kết many-to-many
            entity.OrganizationEntityEmployees = employees
                .Select(e => new OrganizationEntityEmployee
                {
                    EmployeeId = e.Id,
                    OrganizationEntity = entity
                })
                .ToList();
        }

        //  Add và commit
        _context.OrganizationEntities.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<OrganizationEntitySummaryDTO>(entity);
    }

    public override async Task<OrganizationEntitySummaryDTO?> GetByIdAsync(int id)
    {
        // 1) Lấy entity gốc (không include navigation)
        var entity = await _context
            .OrganizationEntities.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
            throw new KeyNotFoundException($"OrganizationEntity with Id={id} not found.");

        // 2) Map các property cơ bản
        var dto = _mapper.Map<OrganizationEntitySummaryDTO>(entity);

        // 3) Lấy parentId & parentName nếu có
        if (entity.ParentId.HasValue)
        {
            var parent = await _context
                .OrganizationEntities.AsNoTracking()
                .Where(x => x.Id == entity.ParentId.Value)
                .Select(x => new { x.Id, x.Name })
                .FirstOrDefaultAsync();

            dto.ParentId = parent?.Id.ToString();
            dto.ParentName = parent?.Name;
        }

        // 4) Lấy danh sách immediate children (chỉ Id & Name)
        var children = await _context
            .OrganizationEntities.AsNoTracking()
            .Where(x => x.ParentId == id)
            .Select(x => new { x.Id, x.Name })
            .ToListAsync();

        dto.ChildrenIds = children.Select(c => c.Id.ToString()).ToList();
        dto.ChildrenNames = children.Select(c => c.Name).ToList();

        // 5) Lấy danh sách employees gắn với entity này
        //    Giả sử bạn có table liên kết OrganizationEntityEmployees
        var employees = await _context
            .OrganizationEntityEmployees.AsNoTracking()
            .Where(oe => oe.OrganizationEntityId == id)
            .Select(oe => new
            {
                oe.Employee.Id,
                oe.Employee.FirstName,
                oe.Employee.MiddleName,
                oe.Employee.LastName
            })
            .ToListAsync();

        dto.EmployeeIds = employees.Select(e => e.Id.ToString()).ToList();
        dto.EmployeeNames = employees
            .Select(e => $"{e.FirstName} {e.MiddleName} {e.LastName}")
            .ToList();

        return dto;
    }

    public override async Task<OrganizationEntitySummaryDTO?> UpdateAsync(
        int id,
        UpdateOrganizationEntityDTO dto
    )
    {
        var entity = await _context
            .OrganizationEntities.Include(o => o.Children)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (entity == null)
            return null;

        // Map các field cơ bản (ngoại trừ Id)
        _mapper.Map(dto, entity);

        // Kiểm tra và gán lại Parent nếu client có gửi
        if (dto.ParentId != null)
        {
            if (dto.ParentId == entity.Id)
                throw new InvalidOperationException("Không thể tự làm cha của mình.");
            var parent = await _context.OrganizationEntities.FindAsync(dto.ParentId.Value);
            if (parent == null)
                throw new InvalidOperationException("Entity này không tồn tại.");
            entity.Parent = parent;
        }
        else
        {
            // nếu client gửi parentId = null, clear quan hệ
            entity.Parent = null;
        }

        //  Kiểm tra và gán lại Children nếu client có gửi
        if (dto.ChildrenIds != null)
        {
            var children = await _context
                .OrganizationEntities.Where(o => dto.ChildrenIds.Contains(o.Id))
                .ToListAsync();
            if (children.Count != dto.ChildrenIds.Count)
                throw new InvalidOperationException("Entity này không tồn tại.");
            entity.Children = children;
        }

        // Xử lý EmployeeIds
        if (dto.EmployeeIds != null)
        {
            var employees = await _context
                .Employees.Where(e => dto.EmployeeIds.Contains(e.Id))
                .ToListAsync();

            if (employees.Count != dto.EmployeeIds.Count)
                throw new KeyNotFoundException("Một hoặc nhiều employeeId không tồn tại.");

            // Xóa hết liên kết cũ
            _context.OrganizationEntityEmployees.RemoveRange(entity.OrganizationEntityEmployees);

            // Tạo lại liên kết mới
            entity.OrganizationEntityEmployees = employees
                .Select(e => new OrganizationEntityEmployee
                {
                    EmployeeId = e.Id,
                    OrganizationEntityId = entity.Id
                })
                .ToList();
        }

        await _context.SaveChangesAsync();
        return _mapper.Map<OrganizationEntitySummaryDTO>(entity);
    }
}
