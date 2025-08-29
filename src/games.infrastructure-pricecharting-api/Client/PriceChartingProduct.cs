namespace games_infrastructure_pricecharting_api.Client;

public class PriceChartingProduct
{
    public required string Id { get; set; }
    public required string ConsoleName { get; set; }
    public int CibPrice { get; set; }
    public int LoosePrice { get; set; }
    public int NewPrice { get; set; }
    public required string ProductName { get; set; }
}