using igdb_infrastructure.Contexts;
using shared_kernel;
using shared_kernel.Contracts;

namespace igdb_infrastructure;

public class IgdbRepository<TEntity>(CacheDbContext context) :
    Repository<TEntity>(context)
    where TEntity : class;