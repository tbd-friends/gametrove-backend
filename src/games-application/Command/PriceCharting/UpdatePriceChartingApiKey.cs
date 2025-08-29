using Ardalis.Result;
using games_application.Contracts;
using Mediator;

namespace games_application.Command.PriceCharting;

public static class UpdatePriceChartingApiKey
{
    public record Command(string? PriceChartingApiKey) : ICommand<Result>;

    public class Handler(IPricingService pricing) : ICommandHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            if (command.PriceChartingApiKey != null)
            {
                await pricing.SetPricingApiKeyAsync(command.PriceChartingApiKey, cancellationToken);
            }
            else
            {
                await pricing.ClearApiKeyAsync(cancellationToken);
            }

            return Result.Success();
        }
    }
}