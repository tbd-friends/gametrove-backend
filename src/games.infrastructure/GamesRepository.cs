using shared_kernel;
using TbdDevelop.GameTrove.Games.Infrastructure.Contexts;

namespace TbdDevelop.GameTrove.Games.Infrastructure;

public class GamesRepository<TEntity>(GameTrackingContext context) :
    Repository<TEntity>(context)
    where TEntity : class;