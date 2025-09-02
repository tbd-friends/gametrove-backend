namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public short Graphics { get; set; }
    public short Sound { get; set; }
    public short Gameplay { get; set; }
    public short Replayability { get; set; }
    public short OverallRating { get; set; }
    public bool Completed { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public DateTime? LastModified { get; set; }

    public virtual Game Game { get; set; } = null!;
}