using Ardalis.Result;
using games_application.Query.Games.Models;
using games_application.Query.Games.Specifications;
using Mediator;
using shared_kernel;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games;

public static class FetchAllGames
{
    public record Query(
        int Start,
        int Limit,
        string? Search) : IQuery<Result<PagedResultSetDto<GameListDto>>>;

    public class Handler(IRepository<Game> repository) : IQueryHandler<Query, Result<PagedResultSetDto<GameListDto>>>
    {
        public async ValueTask<Result<PagedResultSetDto<GameListDto>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            var matchingGamesCount =
                await repository.CountAsync(new GamesMatchingTermSpec(query.Search),
                    cancellationToken);

            if (matchingGamesCount == 0)
            {
                return Result.NotFound();
            }

            var games = await repository.ListAsync(
                new PagedGamesWithDetailSpec(query.Search, query.Start, query.Limit), cancellationToken);

            return Result.Success(new PagedResultSetDto<GameListDto>
            {
                Data = games,
                Limit = query.Limit,
                Page = query.Start,
                TotalResults = matchingGamesCount
            });
        }
    }
}