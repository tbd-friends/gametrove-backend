using Ardalis.Result;
using games_application.Command.Platforms.Specs;
using Mediator;
using shared_kernel;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Platforms;

public static class MapPlatformsToIgdbPlatforms
{
    public record Command(
        IEnumerable<(Guid PlatformIdentifier, int IgdbPlatformId)> Platforms
    ) : ICommand<Result>;

    public class Handler(IRepository<Platform> repository) : ICommandHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var platforms = await repository.ListAsync(
                new PlatformsByIdentifiersSpec(command.Platforms.Select(s => s.PlatformIdentifier)),
                cancellationToken);

            foreach (var update in command.Platforms)
            {
                var toUpdate = platforms.Single(p => p.Identifier == update.PlatformIdentifier);

                if (toUpdate.Mapping != null)
                {
                    if (toUpdate.Mapping.IgdbPlatformId != update.IgdbPlatformId)
                    {
                        toUpdate.Mapping.IgdbPlatformId = update.IgdbPlatformId;
                    }
                }
                else
                {
                    toUpdate.Mapping = new IgdbPlatformMapping()
                        { PlatformId = toUpdate.Id, IgdbPlatformId = update.IgdbPlatformId };
                }

                await repository.UpdateAsync(toUpdate, cancellationToken);
            }

            return Result.Success();
        }
    }
}