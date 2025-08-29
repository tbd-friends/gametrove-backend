namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class PriceChartingHistory
{
    public int Id { get; set; }
    public int AssociationId { get; set; }
    public DateTime ImportDate { get; set; } = DateTime.UtcNow;
    public required string Name { get; set; }
    public decimal? CompleteInBoxPrice { get; set; }
    public decimal? LoosePrice { get; set; }
    public decimal? NewPrice { get; set; }
    public required string ConsoleName { get; set; }
    public bool IsCurrent { get; set; }

    public virtual PriceChartingGameCopyAssociation Association { get; set; } = null!;
}