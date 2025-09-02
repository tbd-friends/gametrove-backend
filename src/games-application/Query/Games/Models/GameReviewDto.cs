namespace games_application.Query.Games.Models;

public class GameReviewDto
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public short Graphics { get; set; }
    public short Sound { get; set; }
    public short Gameplay { get; set; }
    public short Replayability { get; set; }
    public short OverallRating { get; set; }
    public bool Completed { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime? LastModified { get; set; }
}