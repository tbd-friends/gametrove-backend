using games_application.Contracts;
using games_infrastructure_pricecharting_api.Client;
using games_infrastructure_pricecharting_api.Constants;
using shared_kernel_application.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Pricing;

namespace games_infrastructure_pricecharting_api;

public class PricingService(
    PriceChartingApiClient apiClient,
    ISecretStore secrets,
    ICurrentUserService user) : IPricingService
{
    public async ValueTask<bool> IsPricingEnabled(CancellationToken cancellationToken) =>
        user is { IsAuthenticated: true, UserId: not null } &&
        await secrets.ExistsAsync(Helpers.PriceChartingApiKey(user.UserId), cancellationToken);

    private Lazy<string> Key => new(() =>
        Helpers.PriceChartingApiKey(user.UserId ?? throw new ArgumentNullException($"User Invalid - {user}")));

    public async ValueTask SetPricingApiKeyAsync(string apiKey, CancellationToken cancellationToken)
    {
        await secrets.SetSecretAsync(Key.Value, apiKey, cancellationToken);
    }

    public async ValueTask ClearApiKeyAsync(CancellationToken cancellationToken)
    {
        await secrets.RemoveSecretAsync(Key.Value, cancellationToken);
    }

    public async ValueTask<IEnumerable<Product>> SearchAsync(string term, CancellationToken cancellationToken)
    {
        var apiKey = await secrets.GetSecretAsync(Key.Value, cancellationToken);

        var result = await apiClient.GetProductsAsync(term, apiKey, cancellationToken);

        return result.IsSuccess
            ? result.Value.Select(Helpers.FromPriceChartingProduct)
            : [];
    }

    public async ValueTask<Product> FetchProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var apiKey = await secrets.GetSecretAsync(Key.Value, cancellationToken);

        var result = await apiClient.GetProductByIdAsync(id, apiKey, cancellationToken);

        return result.IsSuccess ? result.Value.FromPriceChartingProduct() : Product.Invalid;
    }
}