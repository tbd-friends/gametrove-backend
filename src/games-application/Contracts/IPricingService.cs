using TbdDevelop.GameTrove.Games.Domain.Pricing;

namespace games_application.Contracts;

public interface IPricingService
{
    ValueTask<bool> IsPricingEnabled(CancellationToken cancellationToken);
    ValueTask SetPricingApiKeyAsync(string apiKey, CancellationToken cancellationToken);
    ValueTask ClearApiKeyAsync(CancellationToken cancellationToken);
    ValueTask<IEnumerable<Product>> SearchAsync(string term, CancellationToken cancellationToken);
    ValueTask<Product> FetchProductByIdAsync(int id, CancellationToken cancellationToken);
}