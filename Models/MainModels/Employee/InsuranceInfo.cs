using System.ComponentModel.DataAnnotations;

namespace portal.Models;

public class InsuranceInfo : BaseModelWithOnlyId
{
    // Foreign key to Employee
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    // Government-issued Social Insurance Code (Mã số BHXH)
    [Required]
    public string InsuranceCode { get; set; } = string.Empty;

    // Where the employee registered their health care (Nơi đăng ký KCB)
    public string RegistrationLocation { get; set; } = string.Empty;

    // When they first started participating in insurance
    public DateTime EffectiveDate { get; set; }

    // Optional: when they stopped (resignation, leave, etc.)
    public DateTime? TerminationDate { get; set; }

    // Current insurance status (e.g. Active, Inactive, Suspended)
    public string InsuranceStatus { get; set; } = "Active";

    // Optional notes for HR (e.g. transferred KCB, exemption periods)
    public string? Note { get; set; }
    // NOTE: ENTERPRISED ARE FIXED % BY LAW SO DO NOT WRITE THIS IN DB
}
