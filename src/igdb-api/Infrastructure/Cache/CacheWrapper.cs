using igdb_api.Clients;
using igdb_api.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Endpoint = igdb_api.Clients.Endpoint;

namespace igdb_api.Infrastructure.Cache;

public interface IIgdbCacheWrapper
{
    Task<GameResponse?> FetchGameById(int id, CancellationToken cancellationToken);
}

public class CacheWrapper(IgdbApiClient client, IDbContextFactory<CacheDbContext> factory) : IIgdbCacheWrapper
{
    public async Task<GameResponse?> FetchGameById(int id, CancellationToken cancellationToken)
    {
        await using var context = await factory.CreateDbContextAsync(cancellationToken);

        var game = context.Set<GameResponse>().FirstOrDefault(g => g.Id == id);

        if (game is not null)
        {
            return game;
        }
        
        var matching = (await client.Query(
            new IGDBQuery<GameResponse>
            {
                Endpoint = Endpoint.Games,
                Where = IgdbLanguage.Where($"id={id}")
            }, cancellationToken))?.SingleOrDefault();

        if (matching is null)
        {
            return null;
        }
            
        await context.AddAsync(matching, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
            
        return matching;
    }
}