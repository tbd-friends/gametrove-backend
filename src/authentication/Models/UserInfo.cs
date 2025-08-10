namespace Authentication.Models;

public sealed record UserInfo
{
    public string Id { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Picture { get; init; } = string.Empty;
    public bool EmailVerified { get; init; }
    public DateTime? LastLogin { get; init; }
    public IReadOnlyList<string> Roles { get; init; } = [];
    public IReadOnlyDictionary<string, object> Claims { get; init; } = new Dictionary<string, object>();
}