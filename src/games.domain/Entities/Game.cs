namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public partial class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int PlatformId { get; set; }
    public int? PublisherId { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public Guid Identifier { get; set; } = Guid.NewGuid();

    private Game(
        string name,
        int platformId)
    {
        Name = name;
        PlatformId = platformId;
    }

    public virtual Platform Platform { get; set; } = null!;
    public virtual Publisher? Publisher { get; set; }
    public virtual ICollection<GameCopy> Copies { get; set; } = new List<GameCopy>();

    public virtual IgdbGameMapping? Mapping { get; set; }
    public virtual PriceChartingGameAverage? Averages { get; set; }
    public virtual Review? Review { get; set; }
}