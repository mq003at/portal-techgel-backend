namespace portal.DTOs;

using System.ComponentModel.DataAnnotations;
using portal.Enums;

public class EmployeeQualificationInfoDTO : BaseModelDTO
{
    public int EmployeeId { get; set; }
    public QualificationType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Institution { get; set; }
    public DateTime? GraduationOrIssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? CertificateNumber { get; set; }
    public string? FileUrl { get; set; }
    public string? Note { get; set; }
}

public class CreateEmployeeQualificationInfoDTO : BaseModelCreateDTO
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public QualificationType Type { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Institution { get; set; }
    public DateTime? GraduationOrIssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? CertificateNumber { get; set; }
    public string? FileUrl { get; set; }
    public string? Note { get; set; }
}

public class UpdateEmployeeQualificationInfoDTO : BaseModelUpdateDTO
{
    public QualificationType? Type { get; set; }
    public string? Name { get; set; }
    public string? Institution { get; set; }
    public DateTime? GraduationOrIssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? CertificateNumber { get; set; }
    public string? FileUrl { get; set; }
    public string? Note { get; set; }
}