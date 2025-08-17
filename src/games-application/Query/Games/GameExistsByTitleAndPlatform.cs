using Ardalis.Result;
using games_application.Query.Games.Specifications;
using games_application.Specifications;
using Mediator;
using shared_kernel;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games;

public static class GameExistsByTitleAndPlatform
{
    public record Query(string Title, Guid PlatformIdentifier) : IQuery<Result>;

    public sealed class Handler(IRepository<Game> repository) : IQueryHandler<Query, Result>
    {
        public async ValueTask<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var match = await repository.ListAsync(
                new GameByTitleAndPlatformIdentifierSpec(query.Title, query.PlatformIdentifier),
                cancellationToken);

            return match.Count != 0 ? Result.Success() : Result.NotFound();
        }
    }
}