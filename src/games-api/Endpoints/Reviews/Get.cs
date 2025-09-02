using FastEndpoints;
using games_application.Query.Games;
using games_application.Query.Games.Models;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Reviews;

public class Get(ISender sender) : Endpoint<Get.Request, Results<Ok<Get.Response>, NotFound>>
{
    public override void Configure()
    {
        Get("games/{identifier:guid}/review");

        Policies("AuthPolicy");

        Summary(g =>
        {
            g.Description = "Get a review of a game title";
            g.Params["identifier"] = "The identifier of the game to get the review for";
        });
    }

    public override async Task<Results<Ok<Response>, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await sender.Send(new FetchReview.Query(req.Identifier), ct);

        return result.IsSuccess
            ? TypedResults.Ok(Response.AsResponse(result.Value))
            : TypedResults.NotFound();
    }

    public class Request
    {
        public Guid Identifier { get; set; }
    }

    public class Response
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public short GraphicsRating { get; set; }
        public short SoundRating { get; set; }
        public short GameplayRating { get; set; }
        public short ReplayabilityRating { get; set; }
        public short OverallRating { get; set; }
        public bool Completed { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? LastModified { get; set; }

        public static Response AsResponse(GameReviewDto review)
        {
            return new Response
            {
                Title = review.Title,
                Content = review.Content,
                GraphicsRating = review.Graphics,
                SoundRating = review.Sound,
                GameplayRating = review.Gameplay,
                ReplayabilityRating = review.Replayability,
                OverallRating = review.OverallRating,
                Completed = review.Completed,
                DateAdded = review.DateAdded,
                LastModified = review.LastModified
            };
        }
    }
}