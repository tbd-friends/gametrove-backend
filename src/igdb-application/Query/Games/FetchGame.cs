using igdb_application.Contracts;
using igdb_application.Query.Games.Models;
using igdb_application.Query.Games.Specifications;
using igdb_domain.DomainEvents;
using igdb_domain.Entities;
using Mediator;
using shared_kernel_infrastructure.Contracts;
using shared_kernel_infrastructure.EventBus;
using shared_kernel;
using shared_kernel.Contracts;

namespace igdb_application.Query.Games;

public static class FetchGame
{
    public record Query(int Id) : IQuery<GameDto>;

    public class Handler(
        IRepository<Game> games,
        IGameService service,
        IEventBus eventBus)
        : IQueryHandler<Query, GameDto>
    {
        public async ValueTask<GameDto> Handle(Query query, CancellationToken cancellationToken)
        {
            var game = await games.FirstOrDefaultAsync(new GameByIdNoTrackingSpec(query.Id), cancellationToken);

            if (game is not null)
            {
                return GameDto.FromGame(game);
            }
            
            await eventBus.PublishAsync(new GameCacheMiss(query.Id));

            game = await service.GetGameByIdAsync(query.Id, cancellationToken);

            return GameDto.FromGame(game);
        }
    }
}