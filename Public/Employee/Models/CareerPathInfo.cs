namespace portal.Models;


// Will fix this in the future for further promotion/demotion
public class CareerPathInfo : BaseModelWithOnlyId
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public string? CurrentPosition { get; set; }
    public string? NextPosition { get; set; }
    public DateTime? ExpectedPromotionDate { get; set; }
    public string? PromotionReason { get; set; }
    public string? DemotionReason { get; set; }
    public DateTime? LastPromotionDate { get; set; }
    public DateTime? LastDemotionDate { get; set; }
}
