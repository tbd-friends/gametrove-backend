namespace TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

public sealed class GameCopyResponseModel : ResponseModelBase
{
    public bool IsPricingLinked { get; set; }
    public decimal? Cost { get; set; }
    public DateTime? PurchasedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string Condition { get; set; } = null!;
    public string? Upc { get; set; }
    protected override string UrlBase { get; set; } = "copies";
}