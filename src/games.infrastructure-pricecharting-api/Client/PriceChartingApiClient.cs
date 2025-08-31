using System.Net.Http.Json;
using System.Text.Json;
using Ardalis.Result;
using games_infrastructure_pricecharting_api.Contracts;

namespace games_infrastructure_pricecharting_api.Client;

public class PriceChartingApiClient(
    HttpClient client)
    : IPriceChartingApiClient
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower
    };

    public async Task<Result<PriceChartingProduct>> GetProductByIdAsync(int id, string apiKey,
        CancellationToken cancellationToken)
    {
        var request = await client.GetAsync($"api/product?t={apiKey}&id={id}", cancellationToken);

        if (!request.IsSuccessStatusCode)
        {
            return Result.NotFound();
        }

        var response =
            await request.Content.ReadFromJsonAsync<PriceChartingProduct>(Options, cancellationToken);

        return response is not null ? Result.Success(response) : Result.Invalid();
    }

    public async Task<Result<IEnumerable<PriceChartingProduct>>> GetProductsAsync(string searchTerm,
        string apiKey,
        CancellationToken cancellationToken)
    {
        var request = await client.GetAsync($"api/products?t={apiKey}&q={searchTerm}", cancellationToken);

        if (!request.IsSuccessStatusCode)
        {
            return Result.NotFound();
        }

        var response =
            await request.Content.ReadFromJsonAsync<PriceChartingResponse<PriceChartingProduct>>(Options,
                cancellationToken);

        return response is not null ? Result.Success(response.Products) : Result.Invalid();
    }

    public async Task<Stream> DownloadCurrentPricingFile(string apiKey, CancellationToken cancellationToken = default)
    {
        var request = await client.GetAsync($"price-guide/download-custom?t={apiKey}", cancellationToken);

        if (!request.IsSuccessStatusCode)
        {
            return Stream.Null;
        }

        return await request.Content.ReadAsStreamAsync(cancellationToken);
    }
}