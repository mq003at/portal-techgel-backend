using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using portal.Enums;

namespace portal.DTOs;

[Owned]
public class GeneralDocumentInfoDTO
{
    [Required, StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DocumentTypeEnum Type { get; set; } = DocumentTypeEnum.DICISION;

    [Required]
    public DocumentStatusEnum Status { get; set; } = DocumentStatusEnum.DRAFT;

    [Required]
    public DocumentSubTypeEnum SubType { get; set; } = DocumentSubTypeEnum.NOTIFICATION;

    [Required]
    public DocumentCategoryEnum Category { get; set; } = DocumentCategoryEnum.EQUIPMENT;

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
