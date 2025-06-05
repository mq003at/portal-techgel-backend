using portal.Enums;

namespace portal.Models;

public class PersonalInfo
{
    public Gender Gender { get; set; }
    public string Address { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public string Nationality { get; set; } = null!;
    public string Birthplace { get; set; } = null!;
    public string EthnicGroup { get; set; } = null!;
    public string? PersonalEmail { get; set; }
    public string? PersonalPhoneNumber { get; set; }

    public string? IdCardNumber { get; set; }
    public DateTime? IdCardIssueDate { get; set; }
    public DateTime? IdCardExpiryDate { get; set; }
    public string? IdCardIssuePlace { get; set; }
}
