namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class PriceChartingHighlight
{
    public int PriceChartingId { get; set; }
    public Guid GameIdentifier { get; set; }
    public string Name { get; set; } = null!;
    public decimal DifferencePercentage { get; set; }
}