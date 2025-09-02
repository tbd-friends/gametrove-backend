using Ardalis.Result;
using Ardalis.Specification;
using igdb_application.Contracts;
using igdb_domain.Entities;
using shared_kernel.Contracts;

namespace igdb_infrastructure_api.Services.Fetchers;

public interface IFetcher
{
    ValueTask<bool> FetchById(int entity, CancellationToken cancellationToken);
}

public class GameFetcher(
    IGameService service,
    IRepository<Game> games
) : IFetcher
{
    public async ValueTask<bool> FetchById(int entity, CancellationToken cancellationToken)
    {
        var result = await service.GetGameByIdAsync(entity, cancellationToken);

        if (result.IsNotFound())
        {
            return false;
        }

        var existing = await games.FirstOrDefaultAsync(new GameByIdSpec(entity), cancellationToken);

        if (existing is not null)
        {
            await games.DeleteAsync(existing, cancellationToken);
        }

        await games.AddAsync(result, cancellationToken);

        return true;
    }
}

public class GameByIdSpec : Specification<Game>, ISingleResultSpecification<Game>
{
    public GameByIdSpec(int entity)
    {
        Query.Where(g => g.Id == entity);
    }
}