using games_application.Contracts;
using Mediator;

namespace games_application.Command.PriceCharting;

public static class BeginPriceChartingUpdate
{
    public record Command : ICommand;

    public class Handler(
        IPricingManagementService management
    ) : ICommandHandler<Command>
    {
        public async ValueTask<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            await management.BeginPriceChartingUpdate(cancellationToken);

            return Unit.Value;
        }
    }
}