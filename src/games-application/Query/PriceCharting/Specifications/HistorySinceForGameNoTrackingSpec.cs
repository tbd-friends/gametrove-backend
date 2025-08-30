using Ardalis.Specification;
using games_application.Query.PriceCharting.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.PriceCharting.Specifications;

public sealed class HistorySinceForGameNoTrackingSpec
    : Specification<PriceChartingGameCopyAssociation, PriceChartingHistoryDto>
{
    public HistorySinceForGameNoTrackingSpec(Guid identifier, DateTime since)
    {
        Query.Include(gc => gc.GameCopy)
            .ThenInclude(g => g.Game)
            .Include(h => h.History)
            .Where(a => a.GameCopy.Game.Identifier == identifier)
            .Select(association =>
                new PriceChartingHistoryDto
                {
                    PriceChartingId = association.PriceChartingId,
                    Name = association.Name,
                    LastUpdated = association.LastUpdated,
                    History = (from h in association.History
                            where h.ImportDate >= since
                            select new PricingHistoryDto(
                                h.Id,
                                h.ImportDate,
                                h.ConsoleName,
                                h.Name,
                                h.CompleteInBoxPrice,
                                h.LoosePrice,
                                h.NewPrice))
                        .AsEnumerable()
                });
    }
}