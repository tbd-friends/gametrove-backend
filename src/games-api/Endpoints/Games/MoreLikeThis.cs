using FastEndpoints;
using games_application.Query.Games;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;

public class MoreLikeThis(ISender sender)
    : Endpoint<MoreLikeThis.Request, Results<Ok<IEnumerable<MoreLikeThis.Result>>, NotFound>>
{
    public override void Configure()
    {
        Get("games/{identifier}/more-like-this");

        Policies("AuthPolicy");

        Summary(g => { g.Description = "Fetch a list of games with names similar to the game you're looking at"; });
    }

    public override async Task<Results<Ok<IEnumerable<Result>>, NotFound>> ExecuteAsync(Request req,
        CancellationToken ct)
    {
        var results = await sender.Send(new FetchGamesLikeThis.Query(req.Identifier), ct);

        return results.IsSuccess
            ? TypedResults.Ok(results.Value.Select(r => new Result
                { Identifier = r.Identifier, Name = r.Name, Platform = r.Platform }))
            : TypedResults.NotFound();
    }

    public class Request
    {
        public Guid Identifier { get; set; }
    }

    public class Result
    {
        public Guid Identifier { get; set; }
        public required string Name { get; set; }
        public required string Platform { get; set; }
    }
}