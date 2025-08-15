namespace TbdDevelop.GameTrove.GameApi.Infrastructure.ResultModels;

public sealed class PlatformResponseModel : ResponseModelBase
{
    public string? Manufacturer { get; set; }
    protected override string UrlBase { get; set; } = "platforms";
}