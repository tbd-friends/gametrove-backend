namespace Games.Infrastructure.ResultModels;

public sealed class GameCopyResultModel : ResultModelBase
{
    public decimal? Cost { get; set; }
    public DateTime? PurchasedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public decimal? CompleteInBoxPrice { get; set; }
    public decimal? LoosePrice { get; set; }
    public decimal? NewPrice { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string Condition { get; set; } = null!;
    public string? Upc { get; set; }
    protected override string UrlBase { get; set; } = "copies";
}