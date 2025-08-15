using System.Text.Json.Serialization;

namespace igdb_api.Infrastructure;

public class GameSummary
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string Summary { get; set; } = null!;
    public IEnumerable<PlatformSummary> Platforms { get; set; } = null!;
    public IEnumerable<GenreSummary> Genres { get; set; } = null!;
    public IEnumerable<ThemeSummary> Themes { get; set; } = null!;
}

public class PlatformSummary
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    [JsonPropertyName("alternative_name")] public string AlternativeName { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public int Generation { get; set; }
}

public class GenreSummary
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class ThemeSummary
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}