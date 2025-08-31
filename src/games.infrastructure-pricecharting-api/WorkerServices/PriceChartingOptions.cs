namespace games_infrastructure_pricecharting_api.WorkerServices;

public class PriceChartingOptions
{
    public required string PricingFileDirectory { get; set; }
    public required string Filter { get; set; }
}