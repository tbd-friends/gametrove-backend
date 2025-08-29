using FastEndpoints;
using games_application.Query.PriceCharting;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.PriceCharting;

public class Search(ISender sender) : Endpoint<Search.Request, Results<Ok<IEnumerable<Search.Response>>, NotFound>>
{
    public override void Configure()
    {
        Get("pricecharting/search");

        Policies("AuthPolicy");

        Summary(g => { g.Description = "Search for games by either Upc or Name"; });
    }

    public override async Task<Results<Ok<IEnumerable<Response>>, NotFound>> ExecuteAsync(Request req,
        CancellationToken ct)
    {
        var results = await sender.Send(new SearchForGamesMatching.Query(req.Upc, req.Name), ct);

        if (!results.IsSuccess || !results.Value.Any())
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(results.Value.Select(g => new Response
        {
            PriceChartingId = g.Id,
            Name = g.Name,
            ConsoleName = g.ConsoleName,
            CompleteInBoxPrice = g.CompleteInBox,
            LoosePrice = g.Loose,
            NewPrice = g.New
        }).AsEnumerable());
    }

    public class Request
    {
        public string? Name { get; set; }
        public string? Upc { get; set; }
    }

    public class Response
    {
        public int PriceChartingId { get; set; }
        public required string Name { get; set; }
        public required string ConsoleName { get; set; }
        public decimal? CompleteInBoxPrice { get; set; }
        public decimal? LoosePrice { get; set; }
        public decimal? NewPrice { get; set; }
    }
}