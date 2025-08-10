using Authentication.Abstractions;
using Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Extensions;

public static class HttpContextExtensions
{
    public static async Task<UserInfo?> GetCurrentUserAsync(this HttpContext context, CancellationToken cancellationToken = default)
    {
        var authService = context.RequestServices.GetService<IAuthenticationService>();
        if (authService == null || context.User?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        return await authService.GetUserInfoFromClaimsAsync(context.User, cancellationToken);
    }

    public static async Task<bool> HasRequiredScopeAsync(this HttpContext context, string requiredScope, CancellationToken cancellationToken = default)
    {
        var authService = context.RequestServices.GetService<IAuthenticationService>();
        if (authService == null || context.User?.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        return await authService.HasRequiredScopeAsync(context.User, requiredScope, cancellationToken);
    }

    public static string? GetUserId(this HttpContext context)
    {
        return context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    }
}