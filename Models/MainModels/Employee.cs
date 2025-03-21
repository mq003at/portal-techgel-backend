namespace portal.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum Gender
{
    Male,
    Female,
    Other
}

public enum EmploymentStatus
{
    Active,
    Inactive,
    OnLeave,
    Terminated,
    Retired
}

public class Employee : BaseModel
{
    // Personal Information
    [Required]
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required, EmailAddress]
    public string PersonalEmail { get; set; } = null!;

    [EmailAddress]
    public string? CompanyEmail { get; set; }

    [Required, Phone]
    public string PhoneNumber { get; set; } = null!;

    [Phone]
    public string? CompanyNumber { get; set; }
    public string? Address { get; set; }

    // Employment Details
    [Required]
    public DateTime StartDate { get; set; }
    public DateTime? ProbationStartDate { get; set; }

    public DateTime? ProbationEndDate { get; set; }

    public DateTime? EndDate { get; set; } // Nullable for active employees

    [Required]
    public EmploymentStatus Status { get; set; } = EmploymentStatus.Inactive;

    [Required]
    public string Position { get; set; } = null!;

    public int? ManagerId { get; set; } // Nullable in case no manager
    public Employee? Manager { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; } = 0.00m;

    public ICollection<EmployeeDivision> EmployeeDivisions { get; set; } =
        new List<EmployeeDivision>();
    public ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } =
        new List<EmployeeDepartment>();
    public ICollection<EmployeeSection> EmployeeSections { get; set; } =
        new List<EmployeeSection>();
    public ICollection<EmployeeUnit> EmployeeUnits { get; set; } = new List<EmployeeUnit>();
    public ICollection<EmployeeTeam> EmployeeTeams { get; set; } = new List<EmployeeTeam>();
}
