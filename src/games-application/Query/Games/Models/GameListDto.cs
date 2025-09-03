namespace games_application.Query.Games.Models;

public record GameListDto : GameDto
{
    public short? OverallRating { get; set; }
    public AveragesDto? Averages { get; set; }

    public record AveragesDto(decimal CompleteDifference, decimal LooseDifference, decimal NewDifference);
}