namespace games_application.Query.Games.Models;

public record GameCopyDto
{
    public Guid Identifier { get; init; }
    public string Name { get; init; }
    public decimal? Cost { get; init; }
    public DateTime? PurchasedDate { get; init; }
    public decimal? EstimatedValue { get; init; }
    public string Condition { get; init; } = null!;
    public string? Upc { get; init; }
    public DateTime UpdatedDate { get; init; }
}