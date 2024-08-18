namespace Games.Infrastructure.ResultModels;

public sealed class PlatformResultModel : ResultModelBase
{
    protected override string UrlBase { get; set; } = "platforms";
}