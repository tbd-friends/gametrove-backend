using FastEndpoints;
using games_application.Command.Games;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;

public class Link(ISender sender)
    : Endpoint<Link.Request, Results<Ok<Guid>, BadRequest>>
{
    public override void Configure()
    {
        Post("games/{identifier}/link");

        Policies("AuthPolicy");

        Summary(s =>
        {
            s.Summary = "Associate an existing game with IGDB";
            s.Params["identifier"] = "The identifier of the existing game";
            s.Params["igdbGameId"] = "The IGDB game Id";
        });
    }

    public override async Task<Results<Ok<Guid>, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await sender.Send(
            new LinkToIgdb.Command(
                req.Identifier,
                req.IgdbGameId
            ), ct);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.BadRequest();
    }

    public class Request
    {
        public required Guid Identifier { get; set; }
        public int IgdbGameId { get; set; }
    }
}