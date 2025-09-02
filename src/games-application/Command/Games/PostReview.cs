using Ardalis.Result;
using games_application.Specifications;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Games;

public static class PostReview
{
    public record Command(
        Guid GameIdentifier,
        string Title,
        string Content,
        short Graphics,
        short Gameplay,
        short Sound,
        short Replayability,
        short OverallRating,
        bool Completed
    ) : ICommand<Result>;

    public class Handler(
        IRepository<Game> games,
        IRepository<Review> reviews) : ICommandHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var game = await games.FirstOrDefaultAsync(
                new GameByIdentifierSpec(command.GameIdentifier),
                cancellationToken);

            ArgumentNullException.ThrowIfNull(game);

            await reviews.AddAsync(
                new Review
                {
                    GameId = game.Id,
                    Title = command.Title,
                    Content = command.Content,
                    Graphics = command.Graphics,
                    Gameplay = command.Gameplay,
                    Sound = command.Sound,
                    Replayability = command.Replayability,
                    OverallRating = command.OverallRating,
                    Completed = command.Completed
                }
                , cancellationToken);
            return Result.Success();
        }
    }
}