namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class IgdbPlatformMapping
{
    public int Id { get; set; }
    public int PlatformId { get; set; }
    public int IgdbPlatformId { get; set; }

    public virtual Platform Platform { get; set; } = null!;
}