using Ardalis.Result;
using games_application.Query.Games.Models;
using games_application.Query.Games.Specifications;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games;

public static class FetchReview
{
    public record Query(Guid Identifier) : IQuery<Result<GameReviewDto>>;

    public class Handler(IRepository<Review> reviews) : IQueryHandler<Query, Result<GameReviewDto>>
    {
        public async ValueTask<Result<GameReviewDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            var review = await reviews.FirstOrDefaultAsync(new GameReviewByGameIdentifierSpec(query.Identifier),
                cancellationToken);

            return review is not null ? Result.Success(review) : Result.NotFound();
        }
    }
}