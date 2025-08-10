using Authentication.Abstractions;
using Authentication.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Authentication.Services;

internal sealed class Auth0TokenService : ITokenService
{
    private readonly HttpClient _httpClient;
    private readonly Auth0Options _options;
    private readonly ILogger<Auth0TokenService> _logger;
    private string? _cachedToken;
    private DateTime _tokenExpiry = DateTime.MinValue;

    public Auth0TokenService(HttpClient httpClient, IOptions<Auth0Options> options, ILogger<Auth0TokenService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
        _httpClient.BaseAddress = new Uri($"https://{_options.Domain}");
    }

    public async Task<string?> GetManagementApiTokenAsync(CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiry)
        {
            return _cachedToken;
        }

        try
        {
            var tokenRequest = new
            {
                client_id = _options.ClientId,
                client_secret = _options.ClientSecret,
                audience = _options.ManagementApiAudience,
                grant_type = "client_credentials"
            };

            var response = await _httpClient.PostAsJsonAsync("/oauth/token", tokenRequest, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get management API token. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            var tokenResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(tokenResponse);
            var root = doc.RootElement;

            _cachedToken = root.GetProperty("access_token").GetString();
            var expiresIn = root.GetProperty("expires_in").GetInt32();
            _tokenExpiry = DateTime.UtcNow.AddSeconds(expiresIn - 60); // Refresh 60 seconds early

            return _cachedToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obtaining management API token");
            return null;
        }
    }

    public async Task<bool> IsTokenValidAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/userinfo");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token");
            return false;
        }
    }

    public Task InvalidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (_cachedToken == token)
        {
            _cachedToken = null;
            _tokenExpiry = DateTime.MinValue;
        }
        return Task.CompletedTask;
    }
}