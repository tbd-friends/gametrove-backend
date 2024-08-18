using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace igdb_api.Clients;


public class IGDBApiClient
{
    private readonly HttpClient _client;
    private readonly IGDBAuthClient _authClient;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _options;
    private readonly IMemoryCache _cache;

    public IGDBApiClient(HttpClient client,
        IGDBAuthClient authClient,
        IConfiguration configuration,
        IMemoryCache cache)
    {
        _client = client;
        _authClient = authClient;
        _configuration = configuration;
        _cache = cache;
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private async Task<bool> Authenticate()
    {
        if (_cache.Get("igdb:bearer") is not null)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", _cache.Get("igdb:bearer") as string);
            _client.DefaultRequestHeaders.Add("Client-ID", _cache.Get("igdb:client-id") as string);

            return true;
        }

        var result = await _authClient.Authorize();

        if (result is null) return false;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", result.access_token);
        _client.DefaultRequestHeaders.Add("Client-ID", _configuration["igdb:clientid"]);

        _cache.Set("igdb:bearer", result.access_token, TimeSpan.FromHours(8));
        _cache.Set("igdb:client-id", _configuration["igdb:clientid"], TimeSpan.FromHours(8));

        return true;
    }

    public async Task<IEnumerable<TResult>?> Query<TResult>(
        IGDBQuery<TResult> query,
        CancellationToken cancellationToken = new()) where TResult : class
    {
        if (!await Authenticate()) return null;

        var response = await GetCachedResponse(query,
            async () =>
            {
                var request = await _client.PostAsync($"/v4/{query.Endpoint}",
                    new StringContent(query, Encoding.UTF8, "plain/text"), cancellationToken);

                if (!request.IsSuccessStatusCode) return null;

                return await request.Content.ReadAsStringAsync(cancellationToken);
            });

        return response is null ? null : JsonSerializer.Deserialize<IEnumerable<TResult>>(response, _options);
    }

    private async Task<string?> GetCachedResponse(string queryString, Func<Task<string?>> fetch)
    {
        var key = $"api{queryString.GetHashCode()}";

        return await _cache.GetOrCreateAsync(key, async _ => await fetch());
    }
}