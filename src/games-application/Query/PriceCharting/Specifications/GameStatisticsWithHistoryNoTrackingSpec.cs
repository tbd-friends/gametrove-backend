using Ardalis.Specification;
using games_application.Query.PriceCharting.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.PriceCharting.Specifications;

public sealed class GameStatisticsWithHistoryNoTrackingSpec
    : Specification<GameCopyPricing, PriceChartingHistoryDto>
{
    public GameStatisticsWithHistoryNoTrackingSpec(Guid identifier, DateTime since)
    {
        Query.Include(gc => gc.GameCopy)
            .ThenInclude(g => g.Game)
            .Include(h => h.Pricing)
            .ThenInclude(p => p.History)
            .Where(a => a.GameCopy.Game.Identifier == identifier)
            .Select(link => new PriceChartingHistoryDto
            {
                PriceChartingId = link.PriceChartingId,
                Name = link.Pricing.Name,
                LastUpdated = link.Pricing.LastUpdated,
                CompleteInBox = link.Pricing.CompleteInBoxPrice,
                Loose = link.Pricing.LoosePrice,
                New = link.Pricing.NewPrice,
                Statistics = new PricingStatisticDto
                {
                    CompleteInBoxPercentageChange =  link.Pricing.Statistics.CompleteInBoxPercentageChange,
                    CompleteInBoxPercentageChange12Months =
                        link.Pricing.Statistics.CompleteInBoxPercentageChange12Months,
                    NewPercentageChange =  link.Pricing.Statistics.NewPercentageChange,
                    NewPercentageChange12Months =  link.Pricing.Statistics.NewPercentageChange12Months,
                    LoosePercentageChange =  link.Pricing.Statistics.LoosePercentageChange,
                    LoosePercentageChange12Months =  link.Pricing.Statistics.LoosePercentageChange12Months,
                },
                History =
                    (from h in link.Pricing.History
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