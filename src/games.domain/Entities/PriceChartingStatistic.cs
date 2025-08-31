namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class PriceChartingStatistic
{
    public int Id { get; set; }
    public int PriceChartingId { get; set; }
    public decimal CompleteInBoxPercentageChange { get; set; }
    public decimal CompleteInBoxPercentageChange12Months { get; set; }
    public decimal LoosePercentageChange { get; set; }
    public decimal LoosePercentageChange12Months { get; set; }
    public decimal NewPercentageChange { get; set; }
    public decimal NewPercentageChange12Months { get; set; }
    public bool IsOutlier { get; set; }

    public virtual PriceChartingSnapshot PriceCharting { get; set; } = null!;
}