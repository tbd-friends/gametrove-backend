using Ardalis.ApiEndpoints;
using igdb_api.Clients;
using Microsoft.AspNetCore.Mvc;
using Endpoint = igdb_api.Clients.Endpoint;

namespace igdb_api.Endpoints.Game;

public class Search : EndpointBaseAsync
    .WithRequest<Search.Parameters>
    .WithActionResult<IEnumerable<Search.Result>>
{
    private readonly IGDBApiClient _client;

    public Search(IGDBApiClient client)
    {
        _client = client;
    }

    [HttpGet("[namespace]")]
    public override async Task<ActionResult<IEnumerable<Result>>> HandleAsync([FromQuery] Parameters parameters,
        CancellationToken cancellationToken = new())
    {
        var matching = await _client.Query(
            new IGDBQuery<GameSummary>
            {
                Endpoint = Endpoint.Games,
                Query = IgdbLanguage.Search(parameters.Term)
            }, cancellationToken);

        if (matching is null) return NotFound();

        if (!string.IsNullOrEmpty(parameters.Platform))
        {
            matching = from r in matching
                where r.Platforms != null && r.Platforms.Any(p =>
                    p.Name.Contains(parameters.Platform, StringComparison.CurrentCultureIgnoreCase))
                select r;
        }

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

    private class GameSummary
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Summary { get; set; } = null!;
        public IEnumerable<PlatformSummary> Platforms { get; set; } = null!;
        public IEnumerable<GenreSummary> Genres { get; set; } = null!;
        public IEnumerable<ThemeSummary> Themes { get; set; } = null!;
    }

    private class PlatformSummary
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    private class GenreSummary
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    private class ThemeSummary
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
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