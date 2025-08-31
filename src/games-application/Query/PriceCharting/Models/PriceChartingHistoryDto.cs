namespace games_application.Query.PriceCharting.Models;

public record PricingHistoryDto(
    int Id,
    DateTime Captured,
    string ConsoleName,
    string Name,
    decimal? CompleteInBox,
    decimal? Loose,
    decimal? New);

public record PricingStatisticDto
{
    public decimal CompleteInBoxPercentageChange { get; set; }
    public decimal CompleteInBoxPercentageChange12Months { get; set; }
    public decimal LoosePercentageChange { get; set; }
    public decimal LoosePercentageChange12Months { get; set; }
    public decimal NewPercentageChange { get; set; }
    public decimal NewPercentageChange12Months { get; set; }
}

public class PriceChartingHistoryDto
{
    public int PriceChartingId { get; set; }
    public required string Name { get; set; }
    public required DateTime LastUpdated { get; set; }
    public decimal? CompleteInBox { get; set; }
    public decimal? Loose { get; set; }
    public decimal? New { get; set; }
    public PricingStatisticDto Statistics { get; set; } = null!;
    public IEnumerable<PricingHistoryDto> History { get; set; } = [];
}