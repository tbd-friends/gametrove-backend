using Ardalis.ApiEndpoints;
using igdb_api.Clients;
using igdb_api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Endpoint = igdb_api.Clients.Endpoint;

namespace igdb_api.Endpoints.Game;

[Route("search")]
public class Search(IGDBApiClient client) : EndpointBaseAsync
    .WithRequest<Search.Parameters>
    .WithActionResult<IEnumerable<Search.Result>>
{
    [HttpGet]
    public override async Task<ActionResult<IEnumerable<Result>>> HandleAsync([FromQuery] Parameters parameters,
        CancellationToken cancellationToken = new())
    {
        var matching = await client.Query(
            new IGDBQuery<GameSummary>
            {
                Endpoint = Endpoint.Games,
                Search = IgdbLanguage.Search($"{parameters.Term}"),
                Where = IgdbLanguage.Where($"platforms.name=*\"{parameters.Platform}\"*"),
                Limit = IgdbLanguage.Limit(15)
            }, cancellationToken);

        if (matching is null) return NotFound();

        return Ok((from r in matching
            select new Result
            {
                Id = r.Id,
                Name = r.Name,
                Summary = r.Summary,
                Platforms = r.Platforms != null && r.Platforms.Any()
                    ? (from platform in r.Platforms
                        select new Result.Platform
                        {
                            Name = platform.Name
                        })
                    : null,
                Genres = r.Genres != null && r.Genres.Any()
                    ? (from genre in r.Genres
                        select new Result.Genre
                        {
                            Name = genre.Name
                        })
                    : null,
                Themes = r.Themes != null && r.Themes.Any()
                    ? (from theme in r.Themes
                        select new Result.Theme
                        {
                            Name = theme.Name
                        })
                    : null
            }).ToList());
    }

    public class Parameters
    {
        public string Term { get; set; } = null!;
        public string? Platform { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public IEnumerable<Platform> Platforms { get; set; } = null!;
        public IEnumerable<Genre> Genres { get; set; } = null!;
        public IEnumerable<Theme> Themes { get; set; } = null!;

        public class Platform
        {
            public string Name { get; set; } = null!;
        }

        public class Genre
        {
            public string Name { get; set; } = null!;
        }

        public class Theme
        {
            public string Name { get; set; } = null!;
        }
    }
}