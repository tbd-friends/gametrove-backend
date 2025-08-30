using Ardalis.Result;
using games_application.Query.PriceCharting.Models;
using games_application.Query.PriceCharting.Specifications;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.PriceCharting;

public static class FetchPricing1YearPricingHistory
{
    public record Query(Guid Identifier) : IQuery<Result<IEnumerable<PriceChartingHistoryDto>>>;

    public class Handler(IRepository<GameCopy> gameCopies)
        : IQueryHandler<Query, Result<IEnumerable<PriceChartingHistoryDto>>>
    {
        private readonly DateTime _earliest = DateTime.UtcNow.AddYears(-1);
        public async ValueTask<Result<IEnumerable<PriceChartingHistoryDto>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            var copies = await gameCopies.ListAsync(
                new GameCopyWithAssociationsNoTrackingSpec(query.Identifier),
                cancellationToken);

            var byPriceChartingId = from x in copies.GroupBy(c => c.PriceChartingAssociation?.PriceChartingId)
                let association = x.First().PriceChartingAssociation
                where association != null && x.Key.HasValue
                select new PriceChartingHistoryDto
                {
                    PriceChartingId = (int)x.Key,
                    Name = association.Name,
                    LastUpdated = association.LastUpdated,
                    History = (from h in association.History
                            where h.ImportDate >= _earliest
                            select new PricingHistoryDto(
                                h.Id,
                                h.ImportDate,
                                h.ConsoleName,
                                h.Name,
                                h.CompleteInBoxPrice,
                                h.LoosePrice,
                                h.NewPrice))
                        .AsEnumerable()
                };

            return Result.Success(byPriceChartingId);
        }
    }
}