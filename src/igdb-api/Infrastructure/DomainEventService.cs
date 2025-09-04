using igdb_application.Command.Caching;
using igdb_domain.DomainEvents;
using Mediator;
using shared_kernel_infrastructure.Contracts;
using shared_kernel_infrastructure.EventBus;

namespace igdb_api.Infrastructure;

public class DomainEventService(IServiceScopeFactory factory) : BackgroundService
{
    private IEventBus _eventBus;
    private ISender _sender;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = factory.CreateAsyncScope();
        _eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        _sender = scope.ServiceProvider.GetRequiredService<ISender>();

        await foreach (var pricingEvent in _eventBus.SubscribeAsync<GameCacheMiss>()
                           .WithCancellation(stoppingToken))
        {
            await ProcessCacheMiss(pricingEvent, stoppingToken);
        }
    }

    private async Task ProcessCacheMiss(GameCacheMiss @event, CancellationToken stoppingToken)
    {
        await _sender.Send(new EnqueueCacheRequest.Command(@event.IgdbGameId, "game"), stoppingToken);
    }
}