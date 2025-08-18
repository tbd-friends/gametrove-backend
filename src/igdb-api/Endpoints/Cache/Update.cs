using Ardalis.ApiEndpoints;
using igdb_api.Infrastructure.Cache;
using igdb_api.Infrastructure.Cache.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace igdb_api.Endpoints.Cache;

public class Update(CacheDbContext context) : EndpointBaseAsync
    .WithRequest<Update.Parameters>
    .WithActionResult
{
    [HttpPost("cache/{id}")]
    public override async Task<ActionResult> HandleAsync([FromRoute] Parameters parameters,
        CancellationToken cancellationToken = new())
    {
        await context.AddAsync(new CacheQueueEntry
        {
            EntityId = parameters.Id,
            EntityType = "game",
            Entered = DateTime.UtcNow
        }, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    public class Parameters
    {
        public int Id { get; set; }
    }
}