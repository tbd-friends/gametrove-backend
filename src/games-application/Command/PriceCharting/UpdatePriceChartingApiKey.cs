using Ardalis.Result;
using games_application.Constants;
using Mediator;
using shared_kernel_application.Contracts;

namespace games_application.Command.PriceCharting;

public static class UpdatePriceChartingApiKey
{
    public record Command(string UserIdentifier, string? PriceChartingApiKey) : ICommand<Result>;

    public class Handler(ISecretStore secrets) : ICommandHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var key = Patterns.PriceChartingApiKey(command.UserIdentifier);

            if (command.PriceChartingApiKey != null)
            {
                await secrets.SetSecretAsync(key, command.PriceChartingApiKey);
            }
            else
            {
                await secrets.RemoveSecretAsync(key);
            }

            return Result.Success();
        }
    }
}