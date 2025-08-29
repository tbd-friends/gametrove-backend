namespace games_application.Constants;

public static class Patterns
{
    public static Func<string, string> PriceChartingApiKey = (identifier) => $"{identifier}_pricecharting-api-key";
}