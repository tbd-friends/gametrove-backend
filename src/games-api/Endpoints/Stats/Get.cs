using FastEndpoints;
using games_application.Query.Statistics;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Stats;

public class Get(ISender sender)
    : EndpointWithoutRequest<Results<Ok<UserSummaryResultModel>, NoContent>>
{
    public override void Configure()
    {
        Get("stats");

        Policies("AuthPolicy");

        Summary(s => { s.Summary = "Retrieve general statistics for the user"; });
    }

    public override async Task<Results<Ok<UserSummaryResultModel>, NoContent>> ExecuteAsync(CancellationToken ct)
    {
        var result = await sender.Send(new FetchUserSummary.Query(), ct);

        if (!result.IsSuccess)
        {
            return TypedResults.NoContent();
        }

        return TypedResults.Ok(new UserSummaryResultModel()
        {
            ConsolesCount = result.Value.ConsolesCount,
            CopiesCount = result.Value.CopiesCount,
            PlatformsCount = result.Value.PlatformsCount,
            GameCount = result.Value.GameCount,
            Wishlisted = 0
        });
    }
}