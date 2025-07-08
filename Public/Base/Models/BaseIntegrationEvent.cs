namespace portal.Models;

public abstract class BaseIntegrationEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    public string TriggeredBy { get; set; } = null!;
}
