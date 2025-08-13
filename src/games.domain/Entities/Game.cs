namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int PlatformId { get; set; }
    public int? PublisherId { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid Identifier { get; set; }

    public virtual Platform Platform { get; set; } = null!;
    public virtual Publisher? Publisher { get; set; }
    public virtual ICollection<GameCopy> Copies { get; set; } = null!;
}