using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using shared_kernel_application.Contracts;
using shared_kernel_infrastructure.Configuration;
using shared_kernel_infrastructure.Contracts;

namespace shared_kernel_infrastructure.Secrets;

public class LocalFileSecretStore(
    IOptions<SecretStoreConfiguration> config,
    IDataProtectionProvider provider)
    : ISecretStore
{
    private readonly string _secretsPath = config.Value.AdditionalSettings?["SecretsPath"] ??
                                           Path.Combine(
                                               Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                               ".secrets", config.Value.ApplicationName);

    private readonly IDataProtector _protector =
        provider.CreateProtector($"{config.Value.ApplicationName}.Secrets");

    public async Task<string> GetSecretAsync(string key, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_secretsPath, $"{key}.secret");

        if (!File.Exists(filePath))
        {
            return string.Empty;
        }

        var encryptedData = await File.ReadAllTextAsync(filePath, cancellationToken);

        return _protector.Unprotect(encryptedData);
    }

    public async Task SetSecretAsync(string key, string value, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(_secretsPath);

        var filePath = Path.Combine(_secretsPath, $"{key}.secret");

        var encryptedData = _protector.Protect(value);

        await File.WriteAllTextAsync(filePath, encryptedData, cancellationToken);
    }

    public Task RemoveSecretAsync(string key, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_secretsPath, $"{key}.secret");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_secretsPath, $"{key}.secret");

        return Task.FromResult(File.Exists(filePath));
    }
}