namespace Games.Infrastructure.ResultModels;

public abstract class ResultModelBase
{
    protected abstract string UrlBase { get; set; }
    public Guid Id { get; set; }

    private string? _url;

    public virtual string Url
    {
        get => _url ?? $"{UrlBase}/{Id}";
        set => _url = value;
    }

    public string Description { get; set; } = null!;
}