using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using portal.Enums;

namespace portal.DTOs;

[Owned]
public class GeneralDocumentInfoDTO
{
    [Required, StringLength(255)]
    public string? Name { get; set; }

    [Required]
    public DocumentTypeEnum Type { get; set; }

    [Required]
    public DocumentStatusEnum Status { get; set; }

    [Required]
    public DocumentSubTypeEnum SubType { get; set; }

    [Required]
    public DocumentCategoryEnum Category { get; set; }

    [Required]
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;

    public int OrganizationEntityResponsibleId { get; set; }
    public string OrganizationEntityResponsibleName { get; set; } = string.Empty;

    public List<string> Tag { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public ICollection<int> GeneralWorkflowIds { get; set; } = new List<int>();
    public ICollection<string> GeneralWorkflowNames { get; set; } = new List<string>();
}
public class CreateGeneralDocumentInfoDTO
{
    [StringLength(255)]
    [Required]
    public string Name { get; set; } = "";

    [Required]
    public DocumentTypeEnum Type { get; set; }

    [Required]
    public DocumentStatusEnum Status { get; set; }

    [Required]
    public DocumentSubTypeEnum SubType { get; set; }

    [Required]
    public DocumentCategoryEnum Category { get; set; }

    [Required]
    public int OwnerId { get; set; }

    public int OrganizationEntityResponsibleId { get; set; }

    public List<string> Tag { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public ICollection<int> GeneralWorkflowIds { get; set; } = new List<int>();
}

public class UpdateGeneralDocumentInfoDTO
{
    [StringLength(255)]
    public string? Name { get; set; }

    public DocumentTypeEnum? Type { get; set; }

    public DocumentStatusEnum? Status { get; set; }

    public DocumentSubTypeEnum? SubType { get; set; }

    public DocumentCategoryEnum? Category { get; set; }

    public int? OwnerId { get; set; }

    public int? OrganizationEntityResponsibleId { get; set; }

    public List<string>? Tag { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? Version { get; set; }

    public ICollection<int>? GeneralWorkflowIds { get; set; }
}