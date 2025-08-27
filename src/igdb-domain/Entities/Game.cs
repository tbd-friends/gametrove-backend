using igdb_domain.Entities.Support;
using igdb_domain.Entities.ValueTypes;

namespace igdb_domain.Entities;

public class Game : IgdbEntityBase
{
    public int FirstReleaseDate { get; set; }

    public List<BasicEntityInfo> Genres { get; set; } = [];
    public List<BasicEntityInfo> Keywords { get; set; } = [];
    public string Name { get; set; } = null!;
    public List<Platform> Platforms { get; set; } = [];
    public List<Image> Screenshots { get; set; } = [];

    public string? Storyline { get; set; } = null!;
    public string Summary { get; set; } = null!;

    public List<BasicEntityInfo> Themes { get; set; } = [];
    public List<Video> Videos { get; set; } = [];
    public List<int> Remakes { get; set; } = [];
}