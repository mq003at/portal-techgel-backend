namespace portal.DTOs;

public class CareerPathInfoDTO : BaseModelWithOnlyIdDTO
{
    public int EmployeeId { get; set; }
    public string? CurrentPosition { get; set; }
    public string? NextPosition { get; set; }
    public DateTime? ExpectedPromotionDate { get; set; }
    public string? PromotionReason { get; set; }
    public string? DemotionReason { get; set; }
    public DateTime? LastPromotionDate { get; set; }
    public DateTime? LastDemotionDate { get; set; }
}
public class CreateCareerPathInfoDTO : BaseModelWithOnlyIdCreateDTO
{
    public int EmployeeId { get; set; }
    public string? CurrentPosition { get; set; }
    public string? NextPosition { get; set; }
    public DateTime? ExpectedPromotionDate { get; set; }
    public string? PromotionReason { get; set; }
    public string? DemotionReason { get; set; }
    public DateTime? LastPromotionDate { get; set; }
    public DateTime? LastDemotionDate { get; set; }
}

public class UpdateCareerPathInfoDTO : BaseModelWithOnlyIdUpdateDTO
{
    public string? CurrentPosition { get; set; }
    public string? NextPosition { get; set; }
    public DateTime? ExpectedPromotionDate { get; set; }
    public string? PromotionReason { get; set; }
    public string? DemotionReason { get; set; }
    public DateTime? LastPromotionDate { get; set; }
    public DateTime? LastDemotionDate { get; set; }
}