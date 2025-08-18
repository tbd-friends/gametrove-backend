using Ardalis.Result;
using games_application.Command.Games.Specifications;
using Mediator;
using shared_kernel;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Games;

public static class LinkToIgdb
{
    public record Command(Guid Identifier, int IgdbGameId) : ICommand<Result<Guid>>;

    public class Handler(IRepository<Game> games) : ICommandHandler<Command, Result<Guid>>
    {
        public async ValueTask<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var game = await games.FirstOrDefaultAsync(new GameByIdentifierSpec(command.Identifier), cancellationToken);

            if (game is null)
            {
                return Result.Conflict();
            }
            
            game.AssociateWithIgdb(command.IgdbGameId);

            await games.UpdateAsync(game, cancellationToken);

            return Result.Success(game.Identifier);
        }
    }
}