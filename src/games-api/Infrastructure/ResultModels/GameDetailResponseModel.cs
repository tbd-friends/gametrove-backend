namespace TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

public sealed class GameDetailResponseModel : GameListResponseModel
{
    public IEnumerable<GameCopyResponseModel> Copies { get; set; } = null!;
    public bool HasReview { get; set; }
    public ReviewResponseModel? Review { get; set; }

    public record ReviewResponseModel(short OverallRating, string Title);
}