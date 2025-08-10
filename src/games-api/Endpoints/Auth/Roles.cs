using Authentication.Abstractions;
using Authentication.Extensions;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Auth;

public sealed class Roles(IAuthenticationService authService) 
    : EndpointWithoutRequest<Results<Ok<IReadOnlyList<string>>, UnauthorizedHttpResult>>
{
    public override void Configure()
    {
        Get("user/roles");
        
        Policies("AuthPolicy");
    }

    public override async Task<Results<Ok<IReadOnlyList<string>>, UnauthorizedHttpResult>> ExecuteAsync(CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();
        
        if (string.IsNullOrEmpty(userId))
        {
            return TypedResults.Unauthorized();
        }

        var roles = await authService.GetUserRolesAsync(userId, ct);
        
        return TypedResults.Ok(roles);
    }
}