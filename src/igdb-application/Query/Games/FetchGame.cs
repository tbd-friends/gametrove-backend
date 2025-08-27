using igdb_application.Contracts;
using igdb_application.Query.Games.Models;
using igdb_application.Query.Games.Specifications;
using igdb_domain.Entities;
using Mediator;
using shared_kernel;

namespace igdb_application.Query.Games;

public static class FetchGame
{
    public record Query(int Id) : IQuery<GameDto>;

    public class Handler(
        IRepository<Game> games,
        IGameService service)
        : IQueryHandler<Query, GameDto>
    {
        public async ValueTask<GameDto> Handle(Query query, CancellationToken cancellationToken)
        {
            var game = await games.FirstOrDefaultAsync(new GameByIdNoTrackingSpec(query.Id), cancellationToken) ??
                       await service.GetGameByIdAsync(query.Id, cancellationToken);

            return GameDto.FromGame(game);
        }
    }
}