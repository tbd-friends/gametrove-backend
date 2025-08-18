using MongoDB.Bson;

namespace igdb_api.Infrastructure.Cache.Models;

public class CacheQueueEntry
{
    public ObjectId Id { get; set; }
    public int EntityId { get; set; }
    public required string EntityType { get; set; }
    public DateTime Entered { get; set; }
    public string? State { get; set; }
}