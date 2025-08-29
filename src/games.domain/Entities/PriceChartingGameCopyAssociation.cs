namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public partial class PriceChartingGameCopyAssociation
{
    public int Id { get; set; }
    public int GameCopyId { get; set; }
    public int PriceChartingId { get; set; }
    public string Name { get; set; } = null!;
    public decimal? CompleteInBoxPrice { get; set; }
    public decimal? LoosePrice { get; set; }
    public decimal? NewPrice { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public virtual GameCopy GameCopy { get; set; } = null!;
    public virtual ICollection<PriceChartingHistory> History { get; set; } = [];

    private PriceChartingGameCopyAssociation(
        int gameCopyId,
        int priceChartingId,
        string name,
        decimal? completeInBoxPrice,
        decimal? loosePrice,
        decimal? newPrice
    )
    {
        GameCopyId = gameCopyId;
        PriceChartingId = priceChartingId;
        Name = name;
        CompleteInBoxPrice = completeInBoxPrice;
        LoosePrice = loosePrice;
        NewPrice = newPrice;
    }
}