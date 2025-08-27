using igdb_domain.Entities.Support;

namespace igdb_domain.Entities.ValueTypes;

public class GameSummary : IgdbEntityBase
{
    public string Name { get; set; } = null!;

    public string Summary { get; set; } = null!;
    public IEnumerable<Platform> Platforms { get; set; } = null!;
    public IEnumerable<BasicEntityInfo> Genres { get; set; } = null!;
    public IEnumerable<BasicEntityInfo> Themes { get; set; } = null!;
}