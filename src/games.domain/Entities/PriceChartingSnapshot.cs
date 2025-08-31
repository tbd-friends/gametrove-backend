namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public partial class PriceChartingSnapshot
{
    public int Id { get; set; }
    public int PriceChartingId { get; set; }
    public string Name { get; set; }
    public string ConsoleName { get; set; }
    public decimal? CompleteInBoxPrice { get; set; }
    public decimal? LoosePrice { get; set; }
    public decimal? NewPrice { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public virtual PriceChartingStatistic Statistics { get; set; } = null!;
    public virtual ICollection<PriceChartingSnapshotHistory> History { get; set; } = [];

    private PriceChartingSnapshot(
        int priceChartingId,
        string name,
        string consoleName,
        decimal? completeInBoxPrice,
        decimal? loosePrice,
        decimal? newPrice
    )
    {
        PriceChartingId = priceChartingId;
        Name = name;
        ConsoleName = consoleName;
        CompleteInBoxPrice = completeInBoxPrice;
        LoosePrice = loosePrice;
        NewPrice = newPrice;
    }
}