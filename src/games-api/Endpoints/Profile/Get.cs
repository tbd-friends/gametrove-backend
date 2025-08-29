using FastEndpoints;
using games_application.Query.Profiles;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using shared_kernel.Extensions;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Profile;

public class Get(ISender sender) : EndpointWithoutRequest<Results<Ok<Get.Result>, NotFound>>
{
    public override void Configure()
    {
        Get("profile");

        Policies("AuthPolicy");

        Summary(g => { g.Description = "Fetch profile for current user, will not return any API Key Values"; });
    }

    public override async Task<Results<Ok<Result>, NotFound>> ExecuteAsync(CancellationToken ct)
    {
        var result = await sender.Send(new FetchUserProfile.Query(User.GetUserIdentifier()), ct);

        return result.IsSuccess
            ? TypedResults.Ok(new Result(
                result.Value.Name,
                result.Value.FavoriteGame,
                result.Value.HasPriceChartingApiKey))
            : TypedResults.NotFound();
    }

    public record Result(string Name, string? FavoriteGame, bool HasPriceChartingApiKey = false);
}