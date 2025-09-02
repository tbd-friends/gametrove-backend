using igdb_application.Query.Models;
using igdb_domain.Entities;

namespace igdb_application.Query.Games.Models;

public class GameDto
{
    public int Id { get; set; }
    public CoverDto? Cover { get; set; }
    public DateTimeOffset ReleaseDate { get; set; }
    public IEnumerable<string> AgeRatings { get; set; } = [];

    public List<BasicInfoDto> Genres { get; set; } = [];
    public List<BasicInfoDto> Keywords { get; set; } = [];
    public string Name { get; set; } = null!;
    public List<PlatformDto> Platforms { get; set; } = [];
    public List<ImageDto> Screenshots { get; set; } = [];

    public string? Storyline { get; set; } = null!;
    public string Summary { get; set; } = null!;

    public List<BasicInfoDto> Themes { get; set; } = [];
    public List<VideoDto> Videos { get; set; } = [];
    public List<int> Remakes { get; set; } = [];

    public static GameDto FromGame(Game game)
    {
        return new GameDto
        {
            Id = game.Id,
            Cover = game.Cover != null
                ? new CoverDto(game.Cover.ImageId, game.Cover.Url, game.Cover.Height, game.Cover.Width,
                    game.Cover.AlphaChannel, game.Cover.Animated)
                : null,
            AgeRatings = game.AgeRatings.Select(a => $"{a.Organization.Name} {a.RatingCategory.Rating}"),
            Name = game.Name,
            Storyline = game.Storyline,
            Summary = game.Summary,
            ReleaseDate = DateTimeOffset.FromUnixTimeSeconds(game.FirstReleaseDate),
            Genres = game.Genres.Select(g => new BasicInfoDto(g.Id, g.Name)).ToList(),
            Keywords = game.Keywords.Select(g => new BasicInfoDto(g.Id, g.Name)).ToList(),
            Platforms = game.Platforms.Select(g => new PlatformDto(g.Id, g.Name, g.AlternativeName, g.Generation))
                .ToList(),
            Screenshots = game.Screenshots.Select(s => new ImageDto(s.ImageId, s.Url, s.Height, s.Width)).ToList(),
            Themes = game.Themes.Select(g => new BasicInfoDto(g.Id, g.Name)).ToList(),
            Videos = game.Videos.Select(g => new VideoDto(g.Id, g.Name, g.VideoId)).ToList(),
        };
    }
}