using games_application.Contracts;
using Mediator;

namespace games_application.Command.PriceCharting;

public static class BeginPriceChartingUpdate
{
    public record Command(string UserIdentifier) : ICommand;

    public class Handler(
        IPricingManagementService management
    ) : ICommandHandler<Command>
    {
        public async ValueTask<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            await management.BeginPriceChartingUpdate(command.UserIdentifier, cancellationToken);

            return Unit.Value;
        }
    }
}