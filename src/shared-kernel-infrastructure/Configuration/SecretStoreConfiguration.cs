namespace shared_kernel_infrastructure.Configuration;

public class SecretStoreConfiguration
{
    public SecretStoreType Type { get; set; }
    public required string ApplicationName { get; set; }
    public Dictionary<string, string>? AdditionalSettings { get; set; }
}

public enum SecretStoreType
{
    Local,
    EnvironmentVariables
}