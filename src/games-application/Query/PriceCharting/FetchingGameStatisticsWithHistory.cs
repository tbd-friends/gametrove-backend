using Ardalis.Result;
using games_application.Query.PriceCharting.Models;
using games_application.Query.PriceCharting.Specifications;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.PriceCharting;

public static class FetchingGameStatisticsWithHistory
{
    public record Query(Guid Identifier) : IQuery<Result<IEnumerable<PriceChartingHistoryDto>>>;

    public class Handler(IRepository<GameCopyPricing> associations)
        : IQueryHandler<Query, Result<IEnumerable<PriceChartingHistoryDto>>>
    {
        private readonly DateTime _earliest = DateTime.UtcNow.AddYears(-1);

        public async ValueTask<Result<IEnumerable<PriceChartingHistoryDto>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            var results = await associations.ListAsync(
                new GameStatisticsWithHistoryNoTrackingSpec(query.Identifier, _earliest),
                cancellationToken);

            var uniqueCopies = results
                .DistinctBy(f => f.PriceChartingId)
                .AsEnumerable();

            return Result.Success(uniqueCopies);
        }
    }
}