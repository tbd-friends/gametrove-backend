namespace TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

public sealed class GameDetailResponseModel : GameListResponseModel
{
    public IEnumerable<GameCopyResponseModel> Copies { get; set; } = null!;
}