using FastEndpoints;
using igdb_application.Query.Platforms;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace igdb_api.Endpoints.Platforms;

public class List(ISender sender) : Endpoint<List.Parameters, Results<Ok<IEnumerable<List.Result>>, NotFound>>
{
    public override void Configure()
    {
        Get("platforms");

        Policies("AuthPolicy");

        Summary(s => { s.Description = "List Platforms from IGDB side"; });
    }

    public override async Task<Results<Ok<IEnumerable<Result>>, NotFound>> ExecuteAsync(Parameters parameters,
        CancellationToken ct)
    {
        var results = await sender.Send(new ListPlatforms.Query(parameters.Name), ct);

        return TypedResults.Ok((from r in results.Value
            select new Result
            {
                Id = r.Id,
                Name = r.Name,
                AlternativeName = r.AlternativeName,
            }).AsEnumerable());
    }

    public class Parameters
    {
        public string? Name { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string AlternativeName { get; set; } = null!;
    }
}