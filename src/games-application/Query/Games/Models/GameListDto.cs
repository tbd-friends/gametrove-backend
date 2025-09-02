namespace games_application.Query.Games.Models;

public record GameListDto : GameDto
{
    public short? OverallRating { get; set; }
}