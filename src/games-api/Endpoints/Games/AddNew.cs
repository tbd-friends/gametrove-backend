using FastEndpoints;
using games_application.Command.Games;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;

public class Post(ISender sender)
    : Endpoint<Post.Request, Results<Ok<Guid>, BadRequest>>
{
    public override void Configure()
    {
        Post("games");

        Policies("AuthPolicy");

        Summary(s => { s.Summary = "Register a new game"; });
    }

    public override async Task<Results<Ok<Guid>, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await sender.Send(
            new AddNewGame.Command(
                req.Name,
                req.PlatformIdentifier,
                req.IgdbGameId
            ), ct);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.BadRequest();
    }

    public class Request
    {
        public required string Name { get; set; }
        public required Guid PlatformIdentifier { get; set; }
        public int? IgdbGameId { get; set; }
    }
}