using Ardalis.Specification;
using games_infrastructure_pricecharting_api.WorkerServices.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using shared_kernel_infrastructure.Contracts;
using shared_kernel_infrastructure.EventBus;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_infrastructure_pricecharting_api.WorkerServices;

public class PricingUpdateService(IServiceScopeFactory factory, ILogger<PricingUpdateService> logger)
    : BackgroundService
{
    private IEventBus _eventBus;
    private IRepository<PriceChartingSnapshot> _repository = null!;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _eventBus = factory.CreateScope().ServiceProvider.GetRequiredService<IEventBus>();
        _repository = factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<PriceChartingSnapshot>>();

        await foreach (var pricingEvent in _eventBus.SubscribeAsync<PricingUpdateEvent>()
                           .WithCancellation(stoppingToken))
        {
            await ProcessPricingEvent(pricingEvent, stoppingToken);
        }
    }

    private async Task ProcessPricingEvent(PricingUpdateEvent pricingEvent, CancellationToken stoppingToken)
    {
        logger.LogInformation("Pricing update event received {PriceChartingId}", pricingEvent.PriceChartingId);

        var existing =
            await _repository.FirstOrDefaultAsync(new SnapshotByPriceChartingIdSpec(pricingEvent.PriceChartingId),
                stoppingToken);

        if (existing is null)
        {
            var snapshot = PriceChartingSnapshot.Create(pricingEvent.PriceChartingId,
                pricingEvent.Name,
                pricingEvent.ConsoleName,
                pricingEvent.CompletePrice,
                pricingEvent.LoosePrice,
                pricingEvent.NewPrice);

            await _repository.AddAsync(snapshot, stoppingToken);
        }
        else
        {
            if (existing.LastUpdated < pricingEvent.UpdatedAt)
            {
                existing.UpdateSnapshot(
                    pricingEvent.Name,
                    pricingEvent.ConsoleName,
                    pricingEvent.CompletePrice,
                    pricingEvent.LoosePrice,
                    pricingEvent.NewPrice);
            }
            else
            {
                existing.InsertSnapshotHistory(
                    pricingEvent.Name,
                    pricingEvent.ConsoleName,
                    pricingEvent.CompletePrice,
                    pricingEvent.LoosePrice,
                    pricingEvent.NewPrice,
                    pricingEvent.UpdatedAt);
            }

            await _repository.UpdateAsync(existing, stoppingToken);
        }
    }
}

internal class SnapshotByPriceChartingIdSpec : Specification<PriceChartingSnapshot>,
    ISingleResultSpecification<PriceChartingSnapshot>
{
    public SnapshotByPriceChartingIdSpec(int id)
    {
        Query
            .Include(g => g.History)
            .Where(s => s.PriceChartingId == id);
    }
}