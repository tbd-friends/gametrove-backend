namespace igdb_domain.Entities;

public class CacheQueueEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int EntityId { get; set; }
    public required string EntityType { get; set; }
    public DateTime Entered { get; set; }
    public string? State { get; set; }
}