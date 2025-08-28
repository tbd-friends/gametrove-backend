using Ardalis.Result;
using games_application.Command.Games.Specifications;
using games_application.Specifications;
using Mediator;
using shared_kernel;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Copies;

public static class AddNewCopy
{
    public record Command(
        Guid Identifier,
        DateTime PurchaseDate,
        int Condition,
        decimal? Cost,
        string? Upc)
        : ICommand<Result<Guid>>;

    public class Handler(
        IRepository<Game> games) : ICommandHandler<Command, Result<Guid>>
    {
        public async ValueTask<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var game = await games.FirstOrDefaultAsync(new GameByIdentifierSpec(command.Identifier), cancellationToken);

            ArgumentNullException.ThrowIfNull(game);

            game.AddCopy(command.PurchaseDate, command.Condition, command.Cost, command.Upc);

            await games.UpdateAsync(game, cancellationToken);

            return Result.Success(game.Identifier);
        }
    }
}