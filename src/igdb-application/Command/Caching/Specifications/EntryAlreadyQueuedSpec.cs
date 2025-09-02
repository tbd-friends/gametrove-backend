using Ardalis.Specification;
using igdb_domain.Entities;

namespace igdb_application.Command.Caching.Specifications;

public class EntryAlreadyQueuedSpec : Specification<CacheQueueEntry>, ISingleResultSpecification<CacheQueueEntry>
{
    public EntryAlreadyQueuedSpec(int commandId, string commandEntityType)
    {
        Query.Where(c => c.EntityType == commandEntityType && c.EntityId == commandId);
    }
}