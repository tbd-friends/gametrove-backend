using games_application.Command.PriceCharting;
using Mediator;
using shared_kernel_infrastructure.Contracts;
using shared_kernel_infrastructure.EventBus;
using TbdDevelop.GameTrove.Games.Domain.Events;

namespace TbdDevelop.GameTrove.GameApi.Infrastructure;

public class DomainEventService(IServiceScopeFactory factory) : BackgroundService
{
    private IEventBus _eventBus = null!;
    private ISender _sender = null!;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = factory.CreateAsyncScope();
        _eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        _sender = scope.ServiceProvider.GetRequiredService<ISender>();

        await foreach (var pricingEvent in _eventBus.SubscribeAsync<PricingUpdateRequested>()
                           .WithCancellation(stoppingToken))
        {
            await ProcessPricingRequest(pricingEvent, stoppingToken);
        }
    }

    private async Task ProcessPricingRequest(PricingUpdateRequested @event, CancellationToken stoppingToken)
    {
        await _sender.Send(new BeginPriceChartingUpdate.Command(@event.UserIdentifier), stoppingToken);
    }
}