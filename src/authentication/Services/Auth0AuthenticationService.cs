using Authentication.Abstractions;
using Authentication.Configuration;
using Authentication.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace Authentication.Services;

internal sealed class Auth0AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly Auth0Options _options;
    private readonly ILogger<Auth0AuthenticationService> _logger;

    public Auth0AuthenticationService(
        HttpClient httpClient, 
        ITokenService tokenService,
        IOptions<Auth0Options> options, 
        ILogger<Auth0AuthenticationService> logger)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _options = options.Value;
        _logger = logger;
        _httpClient.BaseAddress = new Uri($"https://{_options.Domain}");
    }

    public async Task<AuthenticationResult> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var isValid = await _tokenService.IsTokenValidAsync(token, cancellationToken);
            if (!isValid)
            {
                return AuthenticationResult.Failure("Invalid token");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, "/userinfo");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return AuthenticationResult.Failure("Failed to get user info");
            }

            var userInfoJson = await response.Content.ReadAsStringAsync(cancellationToken);
            var userInfo = ParseUserInfo(userInfoJson);
            
            return AuthenticationResult.Success(userInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token");
            return AuthenticationResult.Failure("Token validation failed");
        }
    }

    public async Task<UserInfo?> GetUserInfoAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var managementToken = await _tokenService.GetManagementApiTokenAsync(cancellationToken);
            if (string.IsNullOrEmpty(managementToken))
            {
                _logger.LogError("Failed to get management API token");
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/users/{userId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", managementToken);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get user info. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            var userInfoJson = await response.Content.ReadAsStringAsync(cancellationToken);
            return ParseUserInfo(userInfoJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user info for user {UserId}", userId);
            return null;
        }
    }

    public Task<UserInfo?> GetUserInfoFromClaimsAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return Task.FromResult<UserInfo?>(null);
        }

        var userInfo = new UserInfo
        {
            Id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
            Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            Name = principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
            Picture = principal.FindFirst("picture")?.Value ?? string.Empty,
            EmailVerified = bool.TryParse(principal.FindFirst("email_verified")?.Value, out var verified) && verified,
            Roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
            Claims = principal.Claims.ToDictionary(c => c.Type, c => (object)c.Value)
        };

        return Task.FromResult<UserInfo?>(userInfo);
    }

    public Task<bool> HasRequiredScopeAsync(ClaimsPrincipal principal, string requiredScope, CancellationToken cancellationToken = default)
    {
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return Task.FromResult(false);
        }

        var scopes = principal.FindFirst("scope")?.Value?.Split(' ') ?? [];
        return Task.FromResult(scopes.Contains(requiredScope));
    }

    public async Task<IReadOnlyList<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var managementToken = await _tokenService.GetManagementApiTokenAsync(cancellationToken);
            if (string.IsNullOrEmpty(managementToken))
            {
                return [];
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/users/{userId}/roles");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", managementToken);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            var rolesJson = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(rolesJson);
            var roles = new List<string>();

            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var role in doc.RootElement.EnumerateArray())
                {
                    if (role.TryGetProperty("name", out var nameProperty))
                    {
                        roles.Add(nameProperty.GetString() ?? string.Empty);
                    }
                }
            }

            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user roles for user {UserId}", userId);
            return [];
        }
    }

    private static UserInfo ParseUserInfo(string userInfoJson)
    {
        using var doc = JsonDocument.Parse(userInfoJson);
        var root = doc.RootElement;

        return new UserInfo
        {
            Id = root.TryGetProperty("sub", out var sub) ? sub.GetString() ?? string.Empty : string.Empty,
            Email = root.TryGetProperty("email", out var email) ? email.GetString() ?? string.Empty : string.Empty,
            Name = root.TryGetProperty("name", out var name) ? name.GetString() ?? string.Empty : string.Empty,
            Picture = root.TryGetProperty("picture", out var picture) ? picture.GetString() ?? string.Empty : string.Empty,
            EmailVerified = root.TryGetProperty("email_verified", out var emailVerified) && emailVerified.GetBoolean(),
            LastLogin = root.TryGetProperty("updated_at", out var updatedAt) && 
                       DateTime.TryParse(updatedAt.GetString(), out var lastLogin) ? lastLogin : null
        };
    }
}