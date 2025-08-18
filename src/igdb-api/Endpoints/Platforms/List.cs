using Ardalis.ApiEndpoints;
using igdb_api.Clients;
using igdb_api.Infrastructure;
using igdb_api.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Endpoint = igdb_api.Clients.Endpoint;

namespace igdb_api.Endpoints.Platforms;

[Route("platforms")]
public class List(IgdbApiClient client) : EndpointBaseAsync
    .WithRequest<List.Parameters>
    .WithActionResult<IEnumerable<List.Result>>
{
    [HttpGet]
    public override async Task<ActionResult<IEnumerable<Result>>> HandleAsync([FromQuery] Parameters parameters,
        CancellationToken cancellationToken = new())
    {
        var matching = await client.Query(
            new IGDBQuery<PlatformSummary>
            {
                Endpoint = Endpoint.Platforms,
                Where = parameters.Name != null ? IgdbLanguage.Where($"platforms.name=*\"{parameters.Name}\"*") : null,
                Limit = IgdbLanguage.Limit(250)
            }, cancellationToken);

        if (matching is null) return NotFound();

        return Ok((from r in matching
            select new Result
            {
                Id = r.Id,
                Name = r.Name,
                AlternativeName = r.AlternativeName,
            }).ToList());
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