using FastEndpoints;
using games_application.Query.Games;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;

public class ExistsByNameForPlatform(ISender sender) : Endpoint<ExistsByNameForPlatform.Request, Results<Ok, NotFound>>
{
    public override void Configure()
    {
        Head("games/{platformIdentifier}/{name}");
    }

    public override async Task<Results<Ok, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await sender.Send(
            new GameExistsByTitleAndPlatform.Query(req.Name, req.PlatformIdentifier), ct);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.NotFound();
    }

    public class Request
    {
        public required string Name { get; set; }
        public required Guid PlatformIdentifier { get; set; }
    }
}