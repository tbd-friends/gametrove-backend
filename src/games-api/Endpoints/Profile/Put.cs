using FastEndpoints;
using games_application.Command.Profiles;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using shared_kernel.Extensions;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Profile;

public class Put(ISender sender) : Endpoint<Put.Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Put("profile");

        Policies("AuthPolicy");

        Summary(g => { g.Description = "Configure the users profile settings"; });
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await sender.Send(
            new UpdateUserProfile.Command(
                User.GetUserIdentifier(),
                req.Name,
                req.FavoriteGame
            ), ct);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.BadRequest();
    }

    public class Request
    {
        public required string Name { get; set; }
        public string? FavoriteGame { get; set; }
    }
}