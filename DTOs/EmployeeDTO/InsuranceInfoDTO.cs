namespace portal.DTOs;

public class InsuranceInfoDTO
{
    public string? InsuranceNumber { get; set; }
    public string? Provider { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
