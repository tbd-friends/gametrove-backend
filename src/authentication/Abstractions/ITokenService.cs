namespace Authentication.Abstractions;

public interface ITokenService
{
    Task<string?> GetManagementApiTokenAsync(CancellationToken cancellationToken = default);
    Task<bool> IsTokenValidAsync(string token, CancellationToken cancellationToken = default);
    Task InvalidateTokenAsync(string token, CancellationToken cancellationToken = default);
}