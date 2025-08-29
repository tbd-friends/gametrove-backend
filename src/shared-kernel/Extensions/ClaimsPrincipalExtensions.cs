using System.Security.Claims;

namespace shared_kernel.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserIdentifier(this ClaimsPrincipal claimsPrincipal)
    {
        var userIdentifier = claimsPrincipal.Identity?.Name;

        ArgumentException.ThrowIfNullOrEmpty(userIdentifier);

        return userIdentifier;
    }
}