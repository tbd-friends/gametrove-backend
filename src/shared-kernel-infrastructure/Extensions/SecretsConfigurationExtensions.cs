using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using shared_kernel_application.Contracts;
using shared_kernel_infrastructure.Configuration;
using shared_kernel_infrastructure.Contracts;
using shared_kernel_infrastructure.Secrets;

namespace shared_kernel_infrastructure.Extensions;

public static class SecretsConfigurationExtensions
{
    public static THostBuilder AddSecretStore<THostBuilder>(this THostBuilder builder)
        where THostBuilder : IHostApplicationBuilder
    {
        builder.Services.Configure<SecretStoreConfiguration>(
            builder.Configuration.GetSection("SecretStore"));

        builder.Services
            .AddDataProtection()
            .SetApplicationName(builder.Configuration["SecretStore:ApplicationName"]!);

        builder.Services.AddSingleton<LocalFileSecretStore>();

        builder.Services.AddSingleton<ISecretStoreFactory, SecretStoreFactory>();

        builder.Services.AddSingleton<ISecretStore>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<SecretStoreConfiguration>>().Value;
            var factory = provider.GetRequiredService<ISecretStoreFactory>();

            return factory.CreateSecretStore(config);
        });

        return builder;
    }
}