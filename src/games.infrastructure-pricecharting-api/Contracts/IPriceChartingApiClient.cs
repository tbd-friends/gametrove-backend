using Ardalis.Result;
using games_infrastructure_pricecharting_api.Client;

namespace games_infrastructure_pricecharting_api.Contracts;

public interface IPriceChartingApiClient
{
    Task<Result<PriceChartingProduct>> GetProductByIdAsync(int id, string apiKey,
        CancellationToken cancellationToken);

    Task<Result<IEnumerable<PriceChartingProduct>>> GetProductsAsync(string searchTerm, string apiKey,
        CancellationToken cancellationToken);
}