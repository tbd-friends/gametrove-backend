using FastEndpoints;
using games_application.Command.PriceCharting;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Copies;

public class AssociatePricing(ISender sender) : Endpoint<AssociatePricing.Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("copies/{identifier:guid}/associate");

        Policies("AuthPolicy");

        Summary(g => { g.Description = "Associate Price Charting data with this game copy"; });
    }

    public override async Task<Results<Ok, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await sender.Send(new AssociatePricingWithCopy.Command(req.Identifier, req.PriceChartingId), ct);

        return result.IsSuccess ? TypedResults.Ok() : TypedResults.BadRequest();
    }

    public class Request
    {
        public Guid Identifier { get; set; }
        public int PriceChartingId { get; set; }
    }
}