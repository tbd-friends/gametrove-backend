namespace igdb_api.Infrastructure.Models;

public class GameSummary : ApiResponseBase
{
    public string Name { get; set; } = null!;

    public string Summary { get; set; } = null!;
    public IEnumerable<PlatformSummary> Platforms { get; set; } = null!;
    public IEnumerable<BasicEntityInfo> Genres { get; set; } = null!;
    public IEnumerable<BasicEntityInfo> Themes { get; set; } = null!;
}