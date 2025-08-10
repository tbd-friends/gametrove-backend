namespace TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

public sealed class GameDetailResultModel : GameListResultModel
{
    public IEnumerable<GameCopyResultModel> Copies { get; set; } = null!;
}