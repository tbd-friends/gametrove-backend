namespace games_application.Query.Games.Models;

public class SearchResultDto
{
    public Guid Identifier { get; set; }
    public required string Name { get; set; }
    public required string Platform { get; set; }
}