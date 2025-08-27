using System.Text.RegularExpressions;
using FastEndpoints;
using igdb_application.Query.Games;
using igdb_application.Query.Games.Models;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace igdb_api.Endpoints.Game;

public class Get(ISender sender)
    : Endpoint<Get.Parameters, Results<Ok<Get.Result>, NotFound>>
{
    public override void Configure()
    {
        Get("games/{id}");

        Policies("AuthPolicy");

        Summary(s => { s.Description = "Fetch game details from IGDB"; });
    }

    public override async Task<Results<Ok<Result>, NotFound>> ExecuteAsync(Parameters parameters, CancellationToken ct)
    {
        var result = await sender.Send(new FetchGame.Query(parameters.Id), ct);

        if (result is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new Result
        {
            Id = result.Id,
            Name = result.Name,
            Summary = result.Storyline ?? result.Summary,
            Platforms = result.Platforms.Select(p => new Result.Platform { Name = p.Name }),
            Genres = result.Genres.Select(g => new Result.Genre { Name = g.Name }),
            Themes = result.Themes.Select(g => new Result.Theme { Name = g.Name }),
            Screenshots = result.Screenshots.Select(Result.Screenshot.FromImageResponse)
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
            private static readonly Regex _urlRegex = new(@".*upload\/(?<size>.*)\/.*", RegexOptions.Compiled);

            public required string ImageId { get; set; }
            public required string Thumbnail { get; set; }
            public required string Medium { get; set; }

            public int Height { get; set; }
            public int Width { get; set; }

            private static string TransformUrl(string url, string newSize)
            {
                if (string.IsNullOrEmpty(url))
                {
                    return url;
                }

                var match = _urlRegex.Match(url);

                if (!match.Success)
                {
                    return url;
                }

                var currentSize = match.Groups["size"].Value;

                return url.Replace(currentSize, newSize);
            }

            public static Screenshot FromImageResponse(ImageDto response)
            {
                return new Screenshot
                {
                    Thumbnail = response.Url,
                    Medium = TransformUrl(response.Url, "t_720p"),
                    Height = response.Height,
                    Width = response.Width,
                    ImageId = response.ImageId
                };
            }
        }
    }
}