using Ardalis.Result;
using FastEndpoints;
using games_application.Query.Games;
using games_application.Query.Games.Models;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Games;

public class Recent(ISender sender) : EndpointWithoutRequest<Results<Ok<IEnumerable<GameListResponseModel>>, NotFound>>
{
    public override void Configure()
    {
        Get("games/recent");

        Policies("AuthPolicy");

        Summary(g => { g.Description = "Get the most recent 5 updated games"; });
    }

    public override async Task<Results<Ok<IEnumerable<GameListResponseModel>>, NotFound>> ExecuteAsync(
        CancellationToken ct)
    {
        var results =
            BuildResultFromQuery(await sender.Send(new FetchLast5UpdatedGames.Query(), ct))
                .AsEnumerable();

        return results.Any() ? TypedResults.Ok(results) : TypedResults.NotFound();
    }

    private static IEnumerable<GameListResponseModel> BuildResultFromQuery(Result<IEnumerable<GameListDto>> result)
    {
        return result.IsSuccess
            ? from g in result.Value
            select new GameListResponseModel
            {
                Id = g.Identifier,
                IgdbGameId = g.IgdbGameId,
                Description = g.Name,
                OverallRating = g.OverallRating,
                Averages = WithAverages(g),
                Platform = WithPlatform(g),
                Publisher = WithPublisher(g),
                CopyCount = g.CopyCount
            }
            : [];
    }

    private static GameListResponseModel.AveragesResponse? WithAverages(GameListDto g)
    {
        return g.Averages != null
            ? new GameListResponseModel.AveragesResponse(
                g.Averages.CompleteDifference,
                g.Averages.LooseDifference,
                g.Averages.NewDifference)
            : null;
    }

    private static PlatformResponseModel WithPlatform(GameDto g)
    {
        return new PlatformResponseModel
        {
            Id = g.Platform.Identifier,
            Description = g.Platform.Name,
            IgdbPlatformId = g.Platform.IgdbPlatformId
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
}