using System.Text.RegularExpressions;
using Ardalis.ApiEndpoints;
using igdb_api.Clients;
using igdb_api.Infrastructure.Cache;
using igdb_api.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Endpoint = igdb_api.Clients.Endpoint;

namespace igdb_api.Endpoints.Game;

public class FetchGame(IIgdbCacheWrapper cache) : EndpointBaseAsync
    .WithRequest<FetchGame.Parameters>
    .WithActionResult<IEnumerable<FetchGame.Result>>
{
    [HttpGet("games/{id}")]
    public override async Task<ActionResult<IEnumerable<Result>>> HandleAsync([FromRoute] Parameters parameters,
        CancellationToken cancellationToken = new())
    {
        var result = await cache.FetchGameById(parameters.Id, cancellationToken);

        if (result is null) return NotFound();

        return Ok(new Result
        {
            Id = result.Id,
            Name = result.Name,
            Summary = result.Summary,
            Platforms = result.Platforms.Select(p => new Result.Platform { Name = p.Name }),
            Genres = result.Genres.Select(g => new Result.Genre { Name = g.Name }),
            Themes = result.Themes.Select(g => new Result.Theme { Name = g.Name }),
            Screenshots = result.Screenshots.Select(s => new Result.Screenshot
                { Thumbnail = s.Url, Height = s.Height, Width = s.Width, ImageId = s.ImageId })
        });
    }

    public class Parameters
    {
        public int Id { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public IEnumerable<Platform> Platforms { get; set; } = null!;
        public IEnumerable<Genre> Genres { get; set; } = null!;
        public IEnumerable<Theme> Themes { get; set; } = null!;
        public IEnumerable<Screenshot> Screenshots { get; set; } = null!;

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

        public class Screenshot
        {
            private static readonly Regex _urlRegex = new(@".*upload\\/(?<size>.*)\\/.*", RegexOptions.Compiled);

            public string ImageId { get; set; }
            public string Thumbnail { get; set; }
            public string Medium => TransformUrl(Thumbnail, "t_720p");

            public int Height { get; set; }
            public int Width { get; set; }

            private static string TransformUrl(string url, string newSize)
            {
                if (string.IsNullOrEmpty(url))
                    return url;

                return _urlRegex.Replace(url, match =>
                    match.Value.Replace(match.Groups["size"].Value, newSize));
            }
        }
    }
}