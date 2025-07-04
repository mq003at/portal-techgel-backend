using DocumentFormat.OpenXml.Math;

namespace portal.DTOs;

using System.ComponentModel.DataAnnotations;
using portal.Enums;
using portal.Models;

public class OrganizationEntityDTO : BaseModelDTO
{
    public int Level { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public OrganizationStatus Status { get; set; }
    public int? SortOrder { get; set; }

    public int? ParentId { get; set; }
    public string? ParentName { get; set; }

    public List<int> ChildrenIds { get; set; } = new();
    public List<string> ChildrenNames { get; set; } = new();

    public List<int> EmployeeIds { get; set; } = new();
    public List<string> EmployeeMainIds { get; set; } = new();
    public List<string> EmployeeFullNames { get; set; } = new();
    public List<string> EmployeeNames { get; set; } = new();
    public List<EmployeeDTO> Employees { get; set; } = new();

    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }

    public int? DeputyManagerId { get; set; }
    public string? DeputyManagerName { get; set; }
}

public class CreateOrganizationEntityDTO : BaseModelCreateDTO
{
    [Required]
    public int Level { get; set; }

    [Required]
    [MaxLength(50)]
    public string MainId { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
    public OrganizationStatus Status { get; set; } = OrganizationStatus.ACTIVE;

    public int? SortOrder { get; set; }
    public int? ParentId { get; set; }

    // Assign employees during creation
    public List<int> EmployeeIds { get; set; } = new();

    public int? ManagerId { get; set; }
    public int? DeputyManagerId { get; set; }
}

public class UpdateOrganizationEntityDTO : BaseModelUpdateDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public OrganizationStatus? Status { get; set; }
    public int? SortOrder { get; set; }

    public int? ManagerId { get; set; }
    public int? DeputyManagerId { get; set; }
    public string? MainId { get; set; }
}

public class OrganizationEntityUpdateEmployeesDTO : BaseModelUpdateDTO
{
    public int OrganizationEntityId { get; set; }
    public List<int> EmployeeIds { get; set; } = new();
}
