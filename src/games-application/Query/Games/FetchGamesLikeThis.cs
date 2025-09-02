using Ardalis.Result;
using games_application.Query.Games.Models;
using games_application.Query.Games.Specifications;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games;

public static class FetchGamesLikeThis
{
    public record Query(Guid Identifier) : IQuery<Result<IEnumerable<SearchResultDto>>>;

    public class Handler(IRepository<SearchableGame> games) : IQueryHandler<Query, Result<IEnumerable<SearchResultDto>>>
    {
        public async ValueTask<Result<IEnumerable<SearchResultDto>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            var game = await games.FirstOrDefaultAsync(new FetchSearchableGameByIdentifier(query.Identifier),
                cancellationToken);

            ArgumentNullException.ThrowIfNull(game);

            var search = await games.ListAsync(new FindGamesLikeSpec(game.SoundexName), cancellationToken);

            if (!search.Any())
            {
                return Result.NotFound();
            }

            return Result.Success(search.Select(s => new SearchResultDto
                { Identifier = s.Identifier, Name = s.Name, Platform = s.Platform }));
        }
    }
}