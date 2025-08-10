using Authentication.Models;
using System.Security.Claims;

namespace Authentication.Abstractions;

public interface IAuthenticationService
{
    Task<AuthenticationResult> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<UserInfo?> GetUserInfoAsync(string userId, CancellationToken cancellationToken = default);
    Task<UserInfo?> GetUserInfoFromClaimsAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
    Task<bool> HasRequiredScopeAsync(ClaimsPrincipal principal, string requiredScope, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
}