using Ardalis.Result;
using FluentValidation;
using games_application.Command.Games.Specifications;
using games_application.Specifications;
using Mediator;
using shared_kernel;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Games;

public static class AddNewGame
{
    public record Command(
        string Title,
        Guid PlatformIdentifier,
        int? IgdbGameId = null) : ICommand<Result<Guid>>;

    public sealed class Handler(IRepository<Game> games, IRepository<Platform> platforms)
        : ICommandHandler<Command, Result<Guid>>
    {
        public async ValueTask<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var platform =
                await platforms.FirstOrDefaultAsync(new PlatformByIdentifierNoTrackingSpec(command.PlatformIdentifier),
                    cancellationToken);

            ArgumentNullException.ThrowIfNull(platform);

            var game = Game.Create(command.Title, platform.Id);

            if (command.IgdbGameId.HasValue)
            {
                game.AssociateWithIgdb(command.IgdbGameId.Value);
            }

            await games.AddAsync(game, cancellationToken);

            return Result.Success(game.Identifier);
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        private readonly IRepository<Game> _repository;

        public Validator(IRepository<Game> repository)
        {
            _repository = repository;

            RuleFor(x => x)
                .MustAsync(async (request, cancellationToken) =>
                    await NotAlreadyExist(request.Title, request.PlatformIdentifier, cancellationToken))
                .WithMessage("Title already exists");
        }

        private async Task<bool> NotAlreadyExist(string title, Guid platformIdentifier,
            CancellationToken cancellationToken)
        {
            return !await _repository.AnyAsync(
                new GameByTitleAndPlatformIdentifierSpec(title, platformIdentifier),
                cancellationToken);
        }
    }
}