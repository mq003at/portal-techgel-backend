using portal.Enums;

namespace portal.DTOs;

public class PersonalInfoDTO
{
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public string? Birthplace { get; set; }
    public string? EthnicGroup { get; set; }
    public string? Nationality { get; set; }
    public string? PersonalEmail { get; set; }
    public string? PersonalPhoneNumber { get; set; }

    public string? IdCardNumber { get; set; }
    public DateTime? IdCardIssueDate { get; set; }
    public DateTime? IdCardExpiryDate { get; set; }
    public string? IdCardIssuePlace { get; set; }

}
