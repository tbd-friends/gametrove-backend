using FastEndpoints;
using games_application.Command.Games;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Reviews;

public class Post(ISender sender) : Endpoint<Post.Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("games/{identifier:guid}/review");

        Policies("AuthPolicy");

        Summary(g => { g.Description = "Post a review of a game title"; });
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await sender.Send(new PostReview.Command(
            req.Identifier,
            req.Title,
            req.Content,
            req.GraphicsRating,
            req.GameplayRating,
            req.SoundRating,
            req.ReplayabilityRating,
            req.OverallRating,
            req.Completed
        ), ct);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.BadRequest();
    }

    public class Request
    {
        public Guid Identifier { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public short GraphicsRating { get; set; }
        public short SoundRating { get; set; }
        public short GameplayRating { get; set; }
        public short ReplayabilityRating { get; set; }
        public short OverallRating { get; set; }
        public bool Completed { get; set; }
    }
}