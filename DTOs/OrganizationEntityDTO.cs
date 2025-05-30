namespace portal.DTOs;

using System.ComponentModel.DataAnnotations;
using portal.Enums;
using portal.Models;

public class OrganizationEntitySummaryDTO : BaseDTO<OrganizationEntity>
{
    // Tên đơn vị
    public string Name { get; set; } = null!;

    // Mô tả
    public string Description { get; set; } = null!;

    // Thông tin tầng (layer)
    public string LayerId { get; set; } = null!;
    public int Level { get; set; }
    public string LayerName { get; set; } = null!;

    // Người quản lý
    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }

    // Trạng thái đơn vị
    public OrganizationStatus Status { get; set; }

    // Quan hệ cây
    public string? ParentId { get; set; }
    public string? ParentName { get; set; }
    public OrganizationEntitySummaryDTO? Parent { get; set; }

    // Con cháu
    public List<string>? ChildrenIds { get; set; }
    public List<string>? ChildrenNames { get; set; }
    public List<OrganizationEntity>? Children { get; set; } = new List<OrganizationEntity>();

    // Nhân viên
    public List<string>? EmployeeIds { get; set; }
    public List<string>? EmployeeNames { get; set; }
    public List<OrganizationEntity>? Employees { get; set; } = new List<OrganizationEntity>();

    // Thứ tự sắp xếp
    public int? SortOrder { get; set; }

    // Đường dẫn breadcrumb
    public string? FullPathName { get; set; }

    // Đếm tài liệu
    public int DocumentCounts { get; set; }
}

public class CreateOrganizationEntityDTO : BaseDTO<OrganizationEntity>
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public int Level { get; set; }

    public int? ManagerId { get; set; }

    [Required]
    public OrganizationStatus Status { get; set; }

    public int? ParentId { get; set; }
    public List<int>? ChildrenIds { get; set; }
    public List<int>? EmployeeIds { get; set; }
    public int? SortOrder { get; set; }
}

public class UpdateOrganizationEntityDTO : BaseDTO<OrganizationEntity>
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Level { get; set; }

    public int? ManagerId { get; set; }

    public OrganizationStatus? Status { get; set; }

    public int? ParentId { get; set; }
    public OrganizationEntitySummaryDTO? Parent { get; set; }

    public List<int>? ChildrenIds { get; set; }

    public int? SortOrder { get; set; }

    public List<int>? EmployeeIds { get; set; }
}
