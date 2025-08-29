using Ardalis.Result;
using games_application.Contracts;
using games_application.Query.PriceCharting.Models;
using games_application.SharedDtos;
using Mediator;

namespace games_application.Query.PriceCharting;

public static class SearchForGamesMatching
{
    public record Query(string? Upc, string? Name) : IQuery<Result<IEnumerable<PricingDto>>>;

    public class Handler(IPricingService pricing) : IQueryHandler<Query, Result<IEnumerable<PricingDto>>>
    {
        public async ValueTask<Result<IEnumerable<PricingDto>>> Handle(Query query, CancellationToken cancellationToken)
        {
            if (query.Upc == null && query.Name == null)
            {
                return Result.Conflict("No Search Criteria Provided");
            }

            if (!await pricing.IsPricingEnabled(cancellationToken))
            {
                return Result.Forbidden("Pricing is disabled");
            }

            var results = await pricing.SearchAsync((query.Upc ?? query.Name)!, cancellationToken);

            return Result.Success(results.Select(r => r.AsDto()));
        }
    }
}