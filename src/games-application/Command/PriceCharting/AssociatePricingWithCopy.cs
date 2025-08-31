using Ardalis.Result;
using games_application.Command.PriceCharting.Specifications;
using games_application.Contracts;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;
using TbdDevelop.GameTrove.Games.Domain.Pricing;

namespace games_application.Command.PriceCharting;

public static class AssociatePricingWithCopy
{
    public record Command(
        Guid Identifier,
        int PriceChartingId) : ICommand<Result>;

    public class Handler(
        IRepository<GameCopy> copies,
        IPricingService pricing) : ICommandHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var copy = await copies.FirstOrDefaultAsync(new CopyByIdentifierSpec(command.Identifier),
                cancellationToken);

            var product = await pricing.FetchProductByIdAsync(command.PriceChartingId, cancellationToken);

            if (product == Product.Invalid)
            {
                return Result.Error("Unable to retrieve game from PriceCharting");
            }

            ArgumentNullException.ThrowIfNull(copy);

            copy.AssociateWithPriceCharting(PriceChartingSnapshot.Create(command.PriceChartingId,
                product.Name,
                product.ConsoleName,
                product.CompleteInBoxPrice,
                product.LoosePrice,
                product.NewPrice
            ));

            await copies.UpdateAsync(copy, cancellationToken);

            return Result.Success();
        }
    }
}