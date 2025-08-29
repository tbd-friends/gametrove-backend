using shared_kernel_application.Contracts;
using shared_kernel_infrastructure.Configuration;

namespace shared_kernel_infrastructure.Contracts;

public interface ISecretStoreFactory
{
    ISecretStore CreateSecretStore(SecretStoreConfiguration configuration);
}