using FastEndpoints;
using games_application.Command.Games;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;

public class UpdateGameDetails(ISender sender)
    : Endpoint<UpdateGameDetails.Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Put("games/{identifier}");

        Policies("AuthPolicy");

        Summary(s =>
        {
            s.Summary = "Update an existing game details";
            s.Params["identifier"] = "Identifier of the game we're updating";
            s.Params["name"] = "The name of the game";
            s.Params["platformIdentifier"] = "Platform Identifier";
            s.Params["publisherIdentifier"] = "Publisher Identifier";
        });
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await sender.Send(
            new UpdateDetails.Command(
                req.Identifier,
                req.Name,
                req.PlatformId,
                req.PublisherId
            ), ct);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.BadRequest();
    }

    public class Request
    {
        public Guid Identifier { get; set; }
        public required string Name { get; set; }
        public required Guid PlatformId { get; set; }
        public Guid? PublisherId { get; set; }
    }
}