using FastEndpoints;
using games_application.Query.Conditions;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TbdDevelop.GameTrove.GameApi.Endpoints.Conditions;

public class List(ISender sender) : EndpointWithoutRequest<Ok<IEnumerable<List.Result>>>
{
    public override void Configure()
    {
        Get("conditions");

        Policies("AuthPolicy");

        Summary(s => { s.Description = "Fetch list of available conditions"; });
    }

    public override async Task<Ok<IEnumerable<Result>>> ExecuteAsync(CancellationToken ct)
    {
        var results = await sender.Send(new FetchConditions.Query(), ct);

        return TypedResults.Ok(results.Value.Select(r => new Result(r.Value, r.Name)));
    }

    public record Result(int Value, string Label);
}