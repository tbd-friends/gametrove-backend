using Ardalis.Result;
using games_application.Query.Games.Models;
using Mediator;
using shared_kernel;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games;

public static class FetchGamesByConsole
{
    public record Query(
        int Start,
        int PageSize,
        string? Search) : IQuery<Result<PagedResultSetDto<PlatformWithStatsDto>>>;

    public class Handler(IRepository<Game> repository) : IQueryHandler<Query, Result<PagedResultSetDto<PlatformWithStatsDto>>>
    {
        public async ValueTask<Result<PagedResultSetDto<PlatformWithStatsDto>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            return Result.Conflict();
            // var matchingGamesCount =
            //     await repository.CountAsync(new GamesMatchingTermSpec(query.Search),
            //         cancellationToken);
            //
            // if (matchingGamesCount == 0)
            // {
            //     return Result.NotFound();
            // }
            //
            // var games = await repository.ListAsync(
            //     new PagedGamesWithDetailSpec(query.Search, query.Start, query.PageSize), cancellationToken);
            //
            // return Result.Success(new PagedResultSetDto<GameDto>
            // {
            //     Results = games,
            //     PageSize = query.PageSize,
            //     Starting = query.Start,
            //     Total = matchingGamesCount
            // });
        }
    }
}

public class PlatformWithStatsDto
{
}