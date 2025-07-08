namespace portal.DTOs;

public class EmergencyContactInfoDTO : BaseModelWithOnlyIdDTO
{
    public int EmployeeId { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Relationship { get; set; }
    public string? CurrentAddress { get; set; }
}

public class CreateEmergencyContactInfoDTO : BaseModelWithOnlyIdCreateDTO
{
    public int EmployeeId { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Relationship { get; set; }
    public string? CurrentAddress { get; set; }
}

public class UpdateEmergencyContactInfoDTO : BaseModelWithOnlyIdUpdateDTO
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Relationship { get; set; }
    public string? CurrentAddress { get; set; }
}