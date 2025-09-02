using Ardalis.Specification;
using igdb_domain.Entities;

namespace igdb_infrastructure_api.Services.Specifications;

public sealed class QueuedRequestByEnteredDateSpec : Specification<CacheQueueEntry>
{
    public QueuedRequestByEnteredDateSpec()
    {
        Query
            .Where(q => string.IsNullOrEmpty(q.State))
            .OrderBy(q => q.Entered);
    }
}