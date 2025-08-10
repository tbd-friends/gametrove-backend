namespace Authentication.Models;

public sealed record AuthenticationResult
{
    public bool IsAuthenticated { get; init; }
    public UserInfo? User { get; init; }
    public string? ErrorMessage { get; init; }
    public IReadOnlyList<string> Scopes { get; init; } = [];

    public static AuthenticationResult Success(UserInfo user, IReadOnlyList<string>? scopes = null) =>
        new() { IsAuthenticated = true, User = user, Scopes = scopes ?? [] };

    public static AuthenticationResult Failure(string errorMessage) =>
        new() { IsAuthenticated = false, ErrorMessage = errorMessage };
}