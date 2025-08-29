namespace games_infrastructure_pricecharting_api.Client;

public class PriceChartingResponse<TResponse>
    where TResponse : class
{
    public required string Status { get; set; }
    public required IEnumerable<TResponse> Products { get; set; }
}