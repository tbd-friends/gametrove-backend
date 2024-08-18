namespace Games.Infrastructure.Database.Models;

public class PriceChartingGameCopyAssociation
{
    public int Id { get; set; }
    public int GameCopyId { get; set; }
    public int PriceChartingId { get; set; }
    public string Name { get; set; } = null!;
    public decimal? CompleteInBoxPrice { get; set; }
    public decimal? LoosePrice { get; set; }
    public decimal? NewPrice { get; set; }
    public DateTime LastUpdated { get; set; }
}