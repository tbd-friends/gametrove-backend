using igdb_domain.Entities;
using igdb_infrastructure_api.Services.Fetchers;
using igdb_infrastructure_api.Services.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shared_kernel.Contracts;

namespace igdb_infrastructure_api.Services;

public class CacheFetchBackgroundService(
    IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    private readonly Dictionary<string, Type> _fetchers = new()
    {
        { "game", typeof(GameFetcher) }
    };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        while (!stoppingToken.IsCancellationRequested)
        {
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<CacheQueueEntry>>();

            var queue = await repository.ListAsync(new QueuedRequestByEnteredDateSpec(), stoppingToken);

            foreach (var entry in queue)
            {
                if (!await ProcessQueueEntry(stoppingToken, entry))
                {
                    entry.State = "Unable to process entry";

                    await repository.UpdateAsync(entry, stoppingToken);

                    continue;
                }

                await repository.DeleteAsync(entry, stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }

    private async Task<bool> ProcessQueueEntry(
        CancellationToken stoppingToken,
        CacheQueueEntry entry)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        if (scope.ServiceProvider.GetService(_fetchers[entry.EntityType]) is not IFetcher fetcher)
        {
            throw new FetcherUnavailableException(entry.EntityType);
        }

        return await fetcher.FetchById(entry.EntityId, stoppingToken);
    }
}

public class FetcherUnavailableException(string type) : Exception($"{type} Fetcher Unavailable");