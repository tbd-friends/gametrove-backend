using FastEndpoints;
using games_application.Command.PriceCharting;
using games_application.Command.Profiles;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using shared_kernel.Extensions;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.PriceCharting;

public class Profile(ISender sender) : Endpoint<Profile.Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Put("profile/pricecharting");

        Policies("AuthPolicy");

        Summary(g => { g.Description = "Configure the users profile settings"; });
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await sender.Send(
            new UpdatePriceChartingApiKey.Command(
                User.GetUserIdentifier(),
                req.PriceChartingApiKey
            ), ct);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.BadRequest();
    }

    public class Request
    {
        public string? PriceChartingApiKey { get; set; }
    }
}