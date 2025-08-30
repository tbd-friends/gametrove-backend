using FastEndpoints;
using games_application.Query.PriceCharting;
using games_application.Query.PriceCharting.Models;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.PriceCharting;

public class History(ISender sender) : Endpoint<History.Request, Results<Ok<IEnumerable<History.Result>>, NotFound>>
{
    public override void Configure()
    {
        Get("pricecharting/{identifier}/history");

        Policies("AuthPolicy");

        Summary(g =>
        {
            g.Description = "Will fetch the last year of price charting history for the associated types";
        });
    }
    public override async Task<Results<Ok<IEnumerable<Result>>, NotFound>> ExecuteAsync(Request req,
        CancellationToken ct)
    {
        var results = await sender.Send(new FetchPricing1YearPricingHistory.Query(req.Identifier), ct);

        return results.IsSuccess
            ? TypedResults.Ok(
                results.Value.Select(Result.ToDto)
            )
            : TypedResults.NotFound();
    }

    public class Request
    {
        public Guid Identifier { get; set; }
    }

    public class Result
    {
        public int PriceChartingId { get; set; }
        public required string Name { get; set; }
        public DateTime LastUpdated { get; set; }

        public IEnumerable<HistoryResult> History { get; set; } = [];

        public class HistoryResult
        {
            public decimal? CompleteInBox { get; set; }
            public decimal? Loose { get; set; }
            public decimal? New { get; set; }
            public DateTime Captured { get; set; }

            public static HistoryResult AsHistoryResult(PricingHistoryDto dto)
            {
                return new HistoryResult
                {
                    Captured = dto.Captured,
                    Loose = dto.Loose,
                    New = dto.New,
                    CompleteInBox = dto.CompleteInBox,
                };
            }
        }

        public static Result ToDto(PriceChartingHistoryDto dto)
        {
            return new Result
            {
                PriceChartingId = dto.PriceChartingId,
                Name = dto.Name,
                LastUpdated = dto.LastUpdated,
                History = dto.History.Select(HistoryResult.AsHistoryResult)
            };
        }
    }
}