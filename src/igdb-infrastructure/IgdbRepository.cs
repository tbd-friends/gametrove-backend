using igdb_infrastructure.Contexts;
using shared_kernel;

namespace igdb_infrastructure;

public class IgdbRepository<TEntity>(CacheDbContext context) :
    Repository<TEntity>(context)
    where TEntity : class;