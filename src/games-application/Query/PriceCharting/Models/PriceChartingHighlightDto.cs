namespace games_application.Query.PriceCharting.Models;

public class PriceChartingHighlightDto
{
    public required string Name { get; set; }
    public Guid GameIdentifier { get; set; }
    public decimal DifferencePercentage { get; set; }
}