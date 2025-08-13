using FastEndpoints;
using games_application.Query.Games;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using TbdDevelop.GameTrove.GameApi.Infrastructure;
using TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;

public class List(ISender sender)
    : Endpoint<List.Query,
        Results<Ok<ResultSet<GameListResponseModel>>, NotFound>>
{
    public override void Configure()
    {
        Get("games");
        
        Policies("AuthPolicy");
        
        Summary(s =>
        {
            s.Summary = "Get paginated list of games";
            s.Params["page"] = "Page number (default: 0)";
            s.Params["limit"] = "Page size (default: 30)";
            s.Params["search"] = "Search term for filtering games";
        });
    }

    public override async Task<Results<Ok<ResultSet<GameListResponseModel>>, NotFound>> ExecuteAsync(
        Query request,
        CancellationToken ct)
    {
        var result = await sender.Send(new FetchAllGames.Query(
            request.Page,
            request.Limit,
            request.Search
        ), ct);

        if (!result.IsSuccess)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new ResultSet<GameListResponseModel>
        {
            Data = from g in result.Value.Data
                select new GameListResponseModel
                {
                    Id = g.Identifier,
                    Description = g.Name,
                    Platform = new PlatformResponseModel
                    {
                        Id = g.Platform.Identifier,
                        Description = g.Platform.Name,
                    },
                    Publisher = g.Publisher != null
                        ? new PublisherResponseModel
                        {
                            Id = g.Publisher.Identifier,
                            Description = g.Publisher.Name,
                        }
                        : null,
                    CopyCount = g.CopyCount
                },
            Meta = new ResultSet<GameListResponseModel>.MetaData
            {
                Total = result.Value.TotalResults,
                Limit = result.Value.Limit,
                Page = result.Value.Page,
                HasMore = result.Value.HasMore
            }
        });
    }

    public sealed record Query
    {
        public int Page { get; set; }
        public int Limit { get; set; } = 30;
        public string? Search { get; set; }
    }
}