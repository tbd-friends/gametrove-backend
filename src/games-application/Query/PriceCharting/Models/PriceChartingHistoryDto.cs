namespace games_application.Query.PriceCharting.Models;

public record PricingHistoryDto(
    int Id,
    DateTime Captured,
    string ConsoleName,
    string Name,
    decimal? CompleteInBox,
    decimal? Loose,
    decimal? New);

public class PriceChartingHistoryDto
{
    public int PriceChartingId { get; set; }
    public required string Name { get; set; }
    public required DateTime LastUpdated { get; set; }
    public IEnumerable<PricingHistoryDto> History { get; set; } = [];
}