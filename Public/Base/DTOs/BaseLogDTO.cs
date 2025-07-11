namespace portal.DTOs;

public abstract class BaseLogDTO
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
}
