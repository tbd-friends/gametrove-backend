namespace Games.Infrastructure.ResultModels;

public sealed class PublisherResultModel : ResultModelBase
{
    protected override string UrlBase { get; set; } = "publishers";
}