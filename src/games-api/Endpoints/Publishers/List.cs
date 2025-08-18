using FastEndpoints;
using games_application.Query.Platforms;
using games_application.Query.Publishers;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;
using TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Publishers;

public class List(ISender sender)
    : EndpointWithoutRequest<Ok<IEnumerable<PublisherResponseModel>>>
{
    public override void Configure()
    {
        Get("publishers");

        Policies("AuthPolicy");

        Summary(s =>
        {
            s.Summary = "Get list of available publishers";
        });
    }

    public override async Task<Ok<IEnumerable<PublisherResponseModel>>> ExecuteAsync(CancellationToken ct)
    {
        var result = await sender.Send(new FetchAllPublishers.Query(), ct);

        return TypedResults.Ok(result.Value.Select(r => new PublisherResponseModel
        {
            Id = r.Identifier,
            Description = r.Name
        }));
    }
}