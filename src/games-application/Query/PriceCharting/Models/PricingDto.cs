namespace games_application.Query.PriceCharting.Models;

public record PricingDto(int Id, string Name, decimal? CompleteInBox, decimal? Loose, decimal? New);