using FastEndpoints;
using igdb_application.Query.Games;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace igdb_api.Endpoints.Game;

public class Search(ISender sender)
    : Endpoint<Search.Parameters, Results<Ok<IEnumerable<Search.Result>>, NotFound>>
{
    public override void Configure()
    {
        Get("search");

        Policies("AuthPolicy");
    }

    public override async Task<Results<Ok<IEnumerable<Result>>, NotFound>> ExecuteAsync(
        Parameters parameters,
        CancellationToken ct)
    {
        var results = await sender.Send(new SearchGames.Query(parameters.Term, parameters.PlatformId), ct);

        return TypedResults.Ok((from r in results.Value
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
                })
            .AsEnumerable());
    }

    public class Parameters
    {
        public string Term { get; set; } = null!;
        public int PlatformId { get; set; }
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