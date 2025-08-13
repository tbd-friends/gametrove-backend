namespace TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

public class UserSummaryResultModel
{
    public int GameCount { get; set; }
    public int CopiesCount { get; set; }
    public int PlatformsCount { get; set; }
    public int ConsolesCount { get; set; }
    public int Wishlisted { get; set; }
}