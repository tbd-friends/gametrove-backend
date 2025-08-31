namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class GameCopyPricing
{
    public int Id { get; set; }
    public int GameCopyId { get; set; }
    public int PriceChartingId { get; set; }

    public virtual GameCopy GameCopy { get; set; } = null!;
    public virtual PriceChartingSnapshot Pricing { get; set; } = null!;
}