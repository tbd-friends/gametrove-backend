using igdb_api.Clients;
using igdb_api.Infrastructure.Models;
using Endpoint = igdb_api.Clients.Endpoint;

namespace igdb_api.Infrastructure.Cache.Fetchers;

public interface IFetcher
{
    ValueTask<bool> FetchById(int entity, CancellationToken cancellationToken);
}

public class GameFetcher(
    IgdbApiClient client,
    IRepository<Game> games
) : IFetcher
{
    public async ValueTask<bool> FetchById(int entity, CancellationToken cancellationToken)
    {
        var result = await client.Query(
            new IGDBQuery<GameResponse>
            {
                Endpoint = Endpoint.Games,
                Where = IgdbLanguage.Where($"id={entity}")
            }, cancellationToken);

        if (result is null || !result.Any())
        {
            return false;
        }

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        context.Add(result.First());

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}