namespace TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

public class GameListResultModel : ResultModelBase
{
    public PlatformResultModel Platform { get; set; } = null!;
    public PublisherResultModel? Publisher { get; set; }
    public int Copies { get; set; }
    protected override string UrlBase { get; set; } = "games";
}