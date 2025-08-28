using FastEndpoints;
using games_application.Command.Copies;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Copies;

public class Post(ISender sender) : Endpoint<Post.Request, Results<Ok<Guid>, BadRequest>>
{
    public override void Configure()
    {
        Post("games/{identifier:guid}/copies");

        Policies("AuthPolicy");

        Summary(s => { s.Description = "Add a new copy for a game"; });
    }

    public override async Task<Results<Ok<Guid>, BadRequest>> ExecuteAsync(Request request, CancellationToken ct)
    {
        var result = await sender.Send(new AddNewCopy.Command(
            request.Identifier,
            request.PurchasedDate,
            request.Condition,
            request.Cost,
            request.Upc
        ), ct);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.BadRequest();
    }

    public class Request
    {
        public Guid Identifier { get; set; }
        public DateTime PurchasedDate { get; set; }
        public decimal? Cost { get; set; }
        public string? Upc { get; set; }
        public int Condition { get; set; }
    }
}