namespace portal.Models;

using System.ComponentModel.DataAnnotations;
using portal.Enums;
public class EmployeeQualificationInfo : BaseModelWithOnlyId
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    [Required]
    public QualificationType Type { get; set; } // Degree, Certificate, License, Specialization

    [Required]
    public string Name { get; set; } = ""; // e.g., "Bachelor of Electrical Engineering", "AutoCAD Certification"

    public string? Institution { get; set; } // e.g., "HUTECH", "Red Cross", "Autodesk"

    public DateTime? GraduationOrIssueDate { get; set; }

    public DateTime? ExpirationDate { get; set; } // For licenses, certificates

    public string? CertificateNumber { get; set; }

    public string? FileUrl { get; set; } // Scanned copy if uploaded

    public string? Note { get; set; }
}
