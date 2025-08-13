namespace TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

public class GameListResponseModel : ResponseModelBase
{
    public PlatformResponseModel Platform { get; set; } = null!;
    public PublisherResponseModel? Publisher { get; set; }
    public int CopyCount { get; set; }
    protected override string UrlBase { get; set; } = "games";
}