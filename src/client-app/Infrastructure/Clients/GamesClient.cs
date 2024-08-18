using System.Text.Json;
using client_app.Infrastructure.Clients.Models;

namespace client_app.Infrastructure.Clients;

public class GamesClient(HttpClient client)
{
    public async Task<ResultSet<GameListResultModel>> FetchPagedResultSetFromIndex(int start, int pageSize, string? query = null)
    {
        var response = await client.GetAsync($"/games/games?start={start}&pageSize={pageSize}&query={query}");

        if (!response.IsSuccessStatusCode)
        {
            return ResultSet<GameListResultModel>.Empty;
        }

        var content = response.Content.ReadAsStringAsync().Result;

        var results = JsonSerializer.Deserialize<ResultSet<GameListResultModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return results;
    }
}