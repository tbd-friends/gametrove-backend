namespace shared_kernel_application.Contracts;

public interface ISecretStore
{
    Task<string> GetSecretAsync(string key);
    Task SetSecretAsync(string key, string value);
    Task RemoveSecretAsync(string key);
    Task<bool> ExistsAsync(string key);
}