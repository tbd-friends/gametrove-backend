using FastEndpoints;
using games_application.Command.Platforms;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Platforms;

public class Map(ISender sender)
    : Endpoint<Map.Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("platforms/mapping");

        Policies("AuthPolicy");

        Summary(s =>
        {
            s.Summary = "Get list of available platforms";
            s.Params["search"] = "Search term for filtering games";
        });
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request request,
        CancellationToken ct)
    {
        var result =
            await sender.Send(
                new MapPlatformsToIgdbPlatforms.Command(request.Platforms.Select(m =>
                    (m.PlatformIdentifier, m.IgdbPlatformId))), ct);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.BadRequest();
    }

    public class Request
    {
        public IEnumerable<Mapping> Platforms { get; set; } = new List<Mapping>();

        public class Mapping
        {
            public Guid PlatformIdentifier { get; set; }
            public int IgdbPlatformId { get; set; }
        }
    }
}