namespace shared_kernel_application.Contracts;

public interface ISecretStore
{
    Task<string> GetSecretAsync(string key, CancellationToken cancellationToken);
    Task SetSecretAsync(string key, string value, CancellationToken cancellationToken);
    Task RemoveSecretAsync(string key, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken);
}