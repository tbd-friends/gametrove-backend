namespace games_application.Query.Platforms.Models;

public class PlatformResult
{
    public Guid Identifier { get; set; }
    public required string Name { get; set; }
    public string? Manufacturer { get; set; }
}