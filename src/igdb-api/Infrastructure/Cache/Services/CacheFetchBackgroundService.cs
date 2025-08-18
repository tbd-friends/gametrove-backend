using igdb_api.Infrastructure.Cache.Fetchers;
using igdb_api.Infrastructure.Cache.Models;
using Microsoft.EntityFrameworkCore;

namespace igdb_api.Infrastructure.Cache.Services;

public class CacheFetchBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<CacheDbContext>>();

            await using var dbContext = await contextFactory.CreateDbContextAsync(stoppingToken);

            var queue = dbContext.Set<CacheQueueEntry>().Where(e => e.State == null)
                .OrderBy(e => e.Entered);

            foreach (var entry in queue)
            {
                if (!await ProcessQueueEntry(stoppingToken, entry, scope))
                {
                    entry.State = "Unable to process entry";

                    dbContext.Update(entry);
                    
                    await dbContext.SaveChangesAsync(stoppingToken);
                    
                    continue;
                }

                dbContext.Remove(entry);

                await dbContext.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }

    private async Task<bool> ProcessQueueEntry(CancellationToken stoppingToken, CacheQueueEntry entry,
        IServiceScope scope)
    {
        bool hasProcessed = false;

        switch (entry.EntityType)
        {
            case "game":
            {
                var fetch = scope.ServiceProvider.GetService<GameFetcher>();

                if (fetch is null)
                {
                    throw new Exception("Fetcher not fetching");
                }

                hasProcessed = await fetch.FetchById(entry.EntityId, stoppingToken);
            }
                break;
        }

        return hasProcessed;
    }
}