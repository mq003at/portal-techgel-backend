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
