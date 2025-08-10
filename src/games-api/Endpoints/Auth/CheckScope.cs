using Authentication.Extensions;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Auth;

public sealed class CheckScope : Endpoint<CheckScope.Request, Results<Ok<CheckScope.Response>, UnauthorizedHttpResult>>
{
    public override void Configure()
    {
        Get("user/check-scope/{scope}");
        Policies("AuthPolicy");
    }

    public override async Task<Results<Ok<Response>, UnauthorizedHttpResult>> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (!HttpContext.User.Identity?.IsAuthenticated == true)
        {
            return TypedResults.Unauthorized();
        }

        var hasScope = await HttpContext.HasRequiredScopeAsync(req.Scope, ct);
        
        return TypedResults.Ok(new Response { HasScope = hasScope });
    }

    public sealed record Request
    {
        public string Scope { get; set; } = string.Empty;
    }

    public new sealed record Response
    {
        public bool HasScope { get; set; }
    }
}