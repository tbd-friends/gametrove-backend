namespace games_infrastructure_pricecharting_api.WorkerServices.Events;

public class PricingUpdateEvent
{
    public int PriceChartingId { get; set; }
    public string Name { get; set; } = null!;
    public string ConsoleName { get; set; } = null!;
    public decimal LoosePrice { get; set; }
    public decimal CompletePrice { get; set; }
    public decimal NewPrice { get; set; }
    public DateTime UpdatedAt { get; set; }
}