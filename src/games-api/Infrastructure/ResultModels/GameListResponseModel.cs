namespace TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

public class GameListResponseModel : ResponseModelBase
{
    public int? IgdbGameId { get; set; }
    public PlatformResponseModel Platform { get; set; } = null!;
    public PublisherResponseModel? Publisher { get; set; }
    public short? OverallRating { get; set; }
    public int CopyCount { get; set; }
    protected override string UrlBase { get; set; } = "games";
}