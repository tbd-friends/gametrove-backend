using Ardalis.Result;
using games_application.Command.Games.Specifications;
using games_application.Specifications;
using Mediator;
using shared_kernel;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Games;

public static class UpdateDetails
{
    public record Command(
        Guid Identifier,
        string Name,
        Guid PlatformIdentifier,
        Guid? PublisherIdentifier) : ICommand<Result>;

    public class Handler(
        IRepository<Game> games,
        IRepository<Platform> platforms,
        IRepository<Publisher> publishers)
        : ICommandHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var game = await games.FirstOrDefaultAsync(new GameByIdentifierSpec(command.Identifier), cancellationToken);

            var platform =
                await platforms.FirstOrDefaultAsync(new PlatformByIdentifierNoTrackingSpec(command.PlatformIdentifier),
                    cancellationToken);

            var publisher = command.PublisherIdentifier != null
                ? await publishers.FirstOrDefaultAsync(
                    new PublisherByIdentifierNoTrackingSpec(command.PublisherIdentifier.Value), cancellationToken)
                : null;

            if (game is null || platform is null)
            {
                return Result.CriticalError();
            }

            game.UpdateDetails(command.Name, platform.Id, publisher?.Id);

            await games.UpdateAsync(game, cancellationToken);

            return Result.Success();
        }
    }
}