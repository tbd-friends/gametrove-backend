namespace games_application.Query.Games.Models;

public record GameWithCopyDetailDto : GameDto
{
    public bool HasReview { get; set; }
    public ReviewDto? Review { get; set; }
    public IEnumerable<GameCopyDto> Copies { get; set; } = [];

    public record ReviewDto(short OverallRating, string Title);

}