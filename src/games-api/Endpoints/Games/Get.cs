using System.Diagnostics;
using FastEndpoints;
using games_application.Query.Games;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;

public class Get(ISender sender)
    : Endpoint<Get.Query, Results<Ok<GameDetailResponseModel>, NoContent>>
{
    public override void Configure()
    {
        Get("games/{identifier}");

        Policies("AuthPolicy");

        Summary(s =>
        {
            s.Summary = "Get game details by identifier";
            s.Params["identifier"] = "Game identifier (GUID)";
        });
    }

    public override async Task<Results<Ok<GameDetailResponseModel>, NoContent>> ExecuteAsync(Query request,
        CancellationToken ct)
    {
        var result = await sender.Send(new FetchGame.Query(request.Identifier), ct);

        if (!result.IsSuccess)
        {
            return TypedResults.NoContent();
        }

        return TypedResults.Ok(new GameDetailResponseModel
        {
            Id = result.Value.Identifier,
            IgdbGameId = result.Value.IgdbGameId,
            Description = result.Value.Name,
            Platform = new PlatformResponseModel
            {
                Id = result.Value.Platform.Identifier,
                Description = result.Value.Platform.Name,
                IgdbPlatformId = result.Value.Platform.IgdbPlatformId
            },
            Publisher = result.Value.Publisher != null
                ? new PublisherResponseModel
                {
                    Id = result.Value.Publisher.Identifier,
                    Description = result.Value.Publisher.Name,
                }
                : null,
            CopyCount = result.Value.CopyCount,
            Copies = result.Value.Copies.Select(c => new GameCopyResponseModel
            {
                Id = c.Identifier,
                IsPricingLinked = c.IsPriceChartingLinked,
                Description = c.Name,
                Condition = c.Condition,
                Cost = c.Cost,
                PurchasedDate = c.PurchasedDate,
                EstimatedValue = c.EstimatedValue,
                Upc = c.Upc,
                UpdatedDate = c.UpdatedDate
            })
        });
    }

    public sealed record Query
    {
        public Guid Identifier { get; set; }
    }
}