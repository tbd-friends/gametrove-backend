using Microsoft.Extensions.DependencyInjection;
using shared_kernel_application.Contracts;
using shared_kernel_infrastructure.Configuration;
using shared_kernel_infrastructure.Contracts;

namespace shared_kernel_infrastructure.Secrets;

public class SecretStoreFactory(IServiceProvider provider) : ISecretStoreFactory
{
    public ISecretStore CreateSecretStore(SecretStoreConfiguration configuration)
    {
        return configuration.Type switch
        {
            SecretStoreType.Local => provider.GetRequiredService<LocalFileSecretStore>(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}