using Ardalis.Result;
using games_application.Specifications;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Profiles;

public static class UpdateUserProfile
{
    public record Command(
        string UserIdentifier,
        string Name,
        string? FavoriteGame)
        : ICommand<Result>;

    public class Handler(IRepository<Profile> profiles) : ICommandHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var existing = await profiles.FirstOrDefaultAsync(
                new ProfileByUserIdentifierSpec(command.UserIdentifier),
                cancellationToken);

            if (existing is null)
            {
                await profiles.AddAsync(new Profile
                {
                    UserIdentifier = command.UserIdentifier,
                    Name = command.Name,
                    FavoriteGame = command.FavoriteGame
                }, cancellationToken);
            }
            else
            {
                existing.Name = command.Name;
                existing.FavoriteGame = command.FavoriteGame;

                await profiles.UpdateAsync(existing, cancellationToken);
            }

            return Result.Success();
        }
    }
}