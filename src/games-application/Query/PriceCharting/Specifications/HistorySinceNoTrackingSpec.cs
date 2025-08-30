using Ardalis.Specification;
using games_application.Query.PriceCharting.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.PriceCharting.Specifications;

public sealed class HistorySinceNoTrackingSpec : Specification<PriceChartingHistory, PricingHistoryDto>
{
    public HistorySinceNoTrackingSpec(int priceChartingId, DateTime since)
    {
        Query.Where(h => h.AssociationId == priceChartingId && h.ImportDate >= since)
            .AsNoTracking()
            .Select(h =>
                new PricingHistoryDto(
                    h.Id,
                    h.ImportDate,
                    h.ConsoleName,
                    h.Name,
                    h.CompleteInBoxPrice,
                    h.LoosePrice,
                    h.NewPrice));
    }
}