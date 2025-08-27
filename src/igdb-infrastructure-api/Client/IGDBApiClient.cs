using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace igdb_infrastructure_api.Client;

public class IgdbApiClient(
    HttpClient client,
    IgdbAuthClient authClient,
    IConfiguration configuration,
    IMemoryCache cache)
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
    };

    private async Task<bool> Authenticate()
    {
        if (cache.Get("igdb:bearer") is not null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", cache.Get("igdb:bearer") as string);
            client.DefaultRequestHeaders.Add("Client-ID", cache.Get("igdb:client-id") as string);

            return true;
        }

        var result = await authClient.Authorize();

        if (result is null)
        {
            return false;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", result.access_token);
        client.DefaultRequestHeaders.Add("Client-ID", configuration["igdb:clientid"]);

        cache.Set("igdb:bearer", result.access_token, TimeSpan.FromHours(8));
        cache.Set("igdb:client-id", configuration["igdb:clientid"], TimeSpan.FromHours(8));

        return true;
    }

    public async Task<IEnumerable<TResult>?> Query<TResult>(
        IGDBQuery<TResult> query,
        CancellationToken cancellationToken = new()) where TResult : class
    {
        if (!await Authenticate())
        {
            return null;
        }

        var response = await GetCachedResponse(query,
            async () =>
            {
                var request = await client.PostAsync($"/v4/{query.Endpoint}",
                    new StringContent(query, Encoding.UTF8, "plain/text"), cancellationToken);

                if (!request.IsSuccessStatusCode) return null;

                return await request.Content.ReadAsStringAsync(cancellationToken);
            });

        return response is null ? null : JsonSerializer.Deserialize<IEnumerable<TResult>>(response, _options);
    }

    private async Task<string?> GetCachedResponse(string queryString, Func<Task<string?>> fetch)
    {
        var key = $"api{queryString.GetHashCode()}";

        return await cache.GetOrCreateAsync(key, async _ => await fetch(),
            new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5) });
    }
}