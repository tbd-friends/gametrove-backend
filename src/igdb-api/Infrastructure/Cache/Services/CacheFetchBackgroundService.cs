using Ardalis.Specification;
using igdb_api.Infrastructure.Cache.Fetchers;
using igdb_domain.Entities;
using shared_kernel;

namespace igdb_api.Infrastructure.Cache.Services;

public class CacheFetchBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IRepository<CacheQueueEntry>>();

            var queue = await repository.ListAsync(new QueuedRequestByEnteredDateSpec(), stoppingToken);

            foreach (var entry in queue)
            {
                if (!await ProcessQueueEntry(stoppingToken, entry, scope))
                {
                    entry.State = "Unable to process entry";

                    await repository.UpdateAsync(entry, stoppingToken);

                    continue;
                }

                await repository.DeleteAsync(entry, stoppingToken);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }

    private static async Task<bool> ProcessQueueEntry(CancellationToken stoppingToken, CacheQueueEntry entry,
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
                    throw new FetcherUnavailableException(typeof(GameFetcher));
                }

                hasProcessed = await fetch.FetchById(entry.EntityId, stoppingToken);
            }
                break;
        }

        return hasProcessed;
    }
}

public class FetcherUnavailableException(Type type) : Exception($"{type.Name} Unavailable");

public sealed class QueuedRequestByEnteredDateSpec : Specification<CacheQueueEntry>
{
    public QueuedRequestByEnteredDateSpec()
    {
        Query
            .Where(q => q.State == null)
            .OrderBy(q => q.Entered);
    }
}