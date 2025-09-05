using Ardalis.Result;
using games_application.Query.Games.Models;
using games_application.Query.Games.Specifications;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games;

public static class FetchLast5UpdatedGames
{
    public class Query : IQuery<Result<IEnumerable<GameListDto>>>;

    public class Handler(IRepository<Game> games) : IQueryHandler<Query, Result<IEnumerable<GameListDto>>>
    {
        public async ValueTask<Result<IEnumerable<GameListDto>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            var results = await games.ListAsync(new GamesByLastUpdatedDescendingSpec(5), cancellationToken);

            return results.Any() ? Result.Success(results.AsEnumerable()) : Result.NotFound();
        }
    }
}