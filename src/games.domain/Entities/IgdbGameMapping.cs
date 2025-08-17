namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class IgdbGameMapping
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public int IgdbGameId { get; set; }

    public virtual Game Game { get; set; } = null!;
}