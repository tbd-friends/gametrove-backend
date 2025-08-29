namespace games_application.Query.Profiles.Models;

public class ProfileDto
{
    public required string Name { get; set; }
    public string? FavoriteGame { get; set; }
    public bool HasPriceChartingApiKey { get; set; }
}