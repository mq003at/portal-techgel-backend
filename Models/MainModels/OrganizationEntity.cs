using portal.Enums;

namespace portal.Models;

public class OrganizationEntity : BaseModel
{
    // Tương ứng layer.level
    public int Level { get; set; }

    // Tên đơn vị
    public string Name { get; set; } = null!;

    // Mô tả (từ SummaryDTO.description)
    public string Description { get; set; } = null!;

    // Trạng thái (OrganizationStatus)
    public OrganizationStatus Status { get; set; }

    // Thứ tự sort trong cùng 1 parent
    public int? SortOrder { get; set; }

    // Quan hệ cây: Parent ↔ Children
    public int? ParentId { get; set; }
    public OrganizationEntity? Parent { get; set; }
    public List<OrganizationEntity>? Children { get; set; } = new List<OrganizationEntity>();
    public List<int>? ChildrenIds { get; set; } = new List<int>();

    // Người quản lý (Manager) là 1 Employee
    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    // Nhân viên thuộc đơn vị (many-to-many)
    public List<OrganizationEntityEmployee> OrganizationEntityEmployees { get; set; } =
        new List<OrganizationEntityEmployee>();

    // Tính chất view-only, không map xuống DB:
    // - DocumentCounts, FullPathName, EmployeeNames, ParentName, ChildrenNames...
    // Bạn sẽ tính hoặc load riêng trong service layer
}
