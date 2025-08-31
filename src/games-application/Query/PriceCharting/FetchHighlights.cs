using Ardalis.Result;
using games_application.Query.PriceCharting.Models;
using games_application.Query.PriceCharting.Specifications;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.PriceCharting;

public static class FetchHighlights
{
    public record Query : IQuery<Result<IEnumerable<PriceChartingHighlightDto>>>;

    public class Handler(IRepository<PriceChartingHighlight> highlights)
        : IQueryHandler<Query, Result<IEnumerable<PriceChartingHighlightDto>>>
    {
        public async ValueTask<Result<IEnumerable<PriceChartingHighlightDto>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            var results = await highlights.ListAsync(new HighlightsWithNoTracking(), cancellationToken);

            return Result.Success(results.AsEnumerable());
        }
    }
}