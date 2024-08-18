namespace igdb_api.Clients;

public class IGDBAuthClient
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public IGDBAuthClient(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }

    public async Task<AuthorizationResponse?> Authorize()
    {
        var clientId = _configuration["igdb:clientId"];
        var clientSecret = _configuration["igdb:clientSecret"];
        var redirectUri = _configuration["igdb:redirectUri"];

        var request = await _client.PostAsync(
            new Uri(
                $"https://id.twitch.tv/oauth2/token?client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials&redirect_uri={redirectUri}"),
            new StringContent(String.Empty));

        if (request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<AuthorizationResponse>();

            return result;
        }

        return null;
    }

    public class AuthorizationResponse
    {
        public string access_token { get; set; } = null!;
        public int expires_in { get; set; }
        public string token_type { get; set; } = null!;
    }
}