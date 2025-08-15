using Ardalis.Result;
using FastEndpoints;
using games_application.Query.Games;
using games_application.Query.Games.Models;
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

        return TypedResults.Ok(BuildResultFromQuery(result));
    }

    private static ResultSet<GameListResponseModel> BuildResultFromQuery(Result<PagedResultSetDto<GameDto>> result)
    {
        return result.IsSuccess
            ? new ResultSet<GameListResponseModel>
            {
                Data = from g in result.Value.Data
                    select new GameListResponseModel
                    {
                        Id = g.Identifier,
                        Description = g.Name,
                        Platform = WithPlatform(g),
                        Publisher = WithPublisher(g),
                        CopyCount = g.CopyCount
                    },
                Meta = new ResultSet<GameListResponseModel>.MetaData
                {
                    Total = result.Value.TotalResults,
                    Limit = result.Value.Limit,
                    Page = result.Value.Page,
                    HasMore = result.Value.HasMore
                }
            }
            : new ResultSet<GameListResponseModel>() { Data = [] };
    }

    private static PlatformResponseModel WithPlatform(GameDto g)
    {
        return new PlatformResponseModel
        {
            Id = g.Platform.Identifier,
            Description = g.Platform.Name,
        };
    }

    private static PublisherResponseModel? WithPublisher(GameDto g)
    {
        return g.Publisher != null
            ? new PublisherResponseModel
            {
                Id = g.Publisher.Identifier,
                Description = g.Publisher.Name,
            }
            : null;
    }

    public sealed record Query
    {
        public int Page { get; set; }
        public int Limit { get; set; } = 30;
        public string? Search { get; set; }
    }
}