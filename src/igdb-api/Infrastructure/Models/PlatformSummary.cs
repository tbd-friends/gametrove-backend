namespace igdb_api.Infrastructure.Models;

public class PlatformSummary : BasicEntityInfo
{
    public string AlternativeName { get; set; } = null!;
    public int Generation { get; set; }
}