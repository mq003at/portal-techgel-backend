namespace portal.DTOs;

public class EmergencyContactInfoDTO : BaseModelWithOnlyIdDTO
{
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? Relationship { get; set; }
    public string? EmergencyContactCurrentAddress { get; set; }
    public string? EmergencyContactPermanentAddress { get; set; }
}

public class CreateEmergencyContactInfoDTO
{
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? Relationship { get; set; }
    public string? EmergencyContactCurrentAddress { get; set; }
    public string? EmergencyContactPermanentAddress { get; set; }
}

public class UpdateEmergencyContactInfoDTO
{
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? Relationship { get; set; }
    public string? EmergencyContactCurrentAddress { get; set; }
    public string? EmergencyContactPermanentAddress { get; set; }
}