using Ardalis.Result;
using igdb_application.Contracts;
using igdb_application.Query.Games.Models;
using igdb_domain.Entities;
using Mediator;

namespace igdb_application.Query.Games;

public static class SearchGames
{
    public record Query(string Term, int PlatformId) : IQuery<Result<IEnumerable<GameDto>>>;

    public class Handler(IGameService games) : IQueryHandler<Query, Result<IEnumerable<GameDto>>>
    {
        public async ValueTask<Result<IEnumerable<GameDto>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var results = await games.SearchAsync(query.Term, query.PlatformId, cancellationToken);

            return Result.Success(results.Select(GameDto.FromGame));
        }
    }
}