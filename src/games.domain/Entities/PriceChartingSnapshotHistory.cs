namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class PriceChartingSnapshotHistory
{
    public int Id { get; set; }
    public int PriceChartingId { get; set; }
    public required string Name { get; set; }
    public required string ConsoleName { get; set; }
    public decimal? CompleteInBoxPrice { get; set; }
    public decimal? LoosePrice { get; set; }
    public decimal? NewPrice { get; set; }
    public DateTime ImportDate { get; set; }
}