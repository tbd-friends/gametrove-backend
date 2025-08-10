using Authentication.Extensions;
using Authentication.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Auth;

public sealed class Profile : EndpointWithoutRequest<Results<Ok<UserInfo>, UnauthorizedHttpResult>>
{
    public override void Configure()
    {
        Get("user/profile");
        
        Policies("AuthPolicy");
    }

    public override async Task<Results<Ok<UserInfo>, UnauthorizedHttpResult>> ExecuteAsync(CancellationToken ct)
    {
        var userInfo = await HttpContext.GetCurrentUserAsync(ct);
        
        return userInfo != null 
            ? TypedResults.Ok(userInfo) 
            : TypedResults.Unauthorized();
    }
}