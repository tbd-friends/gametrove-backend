using FastEndpoints;
using games_application.Query.PriceCharting;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.PriceCharting;

public class Highlights(ISender sender) : EndpointWithoutRequest<Ok<IEnumerable<Highlights.Result>>>
{
    public override void Configure()
    {
        Get("pricecharting/highlights");

        Policies("AuthPolicy");

        Summary(g =>
        {
            g.Description =
                "Will provide the top 10 games that have significant differences since their last updated";
        });
    }

    public override async Task<Ok<IEnumerable<Result>>> ExecuteAsync(CancellationToken ct)
    {
        var results = await sender.Send(new FetchHighlights.Query(), ct);

        return TypedResults.Ok(results.Value.Select(r => new Result
        {
            Name = r.Name,
            GameIdentifier = r.GameIdentifier,
            DifferencePercentage = r.DifferencePercentage
        }));
    }

    public class Result
    {
        public Guid GameIdentifier { get; set; }
        public required string Name { get; set; }
        public decimal DifferencePercentage { get; set; }
    }
}