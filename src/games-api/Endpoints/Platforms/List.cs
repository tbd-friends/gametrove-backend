using FastEndpoints;
using games_application.Query.Platforms;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Platforms;

public class List(ISender sender)
    : Endpoint<List.Query,
        Ok<IEnumerable<PlatformResponseModel>>>
{
    public override void Configure()
    {
        Get("platforms");

        Policies("AuthPolicy");

        Summary(s =>
        {
            s.Summary = "Get list of available platforms";
            s.Params["search"] = "Search term for filtering games";
        });
    }

    public override async Task<Ok<IEnumerable<PlatformResponseModel>>> ExecuteAsync(
        Query request,
        CancellationToken ct)
    {
        var result = await sender.Send(new FetchAllPlatforms.Query(
            request.Search
        ), ct);

        return TypedResults.Ok(result.Value.Select(r => new PlatformResponseModel
        {
            Id = r.Identifier,
            Description = r.Name,
            Manufacturer = r.Manufacturer,
            IgdbPlatformId = r.IgdbPlatformId
        }));
    }


    public sealed record Query
    {
        public string? Search { get; set; }
    }
}