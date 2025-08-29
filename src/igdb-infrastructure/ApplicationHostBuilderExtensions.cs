using igdb_infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shared_kernel;
using shared_kernel.Contracts;

namespace igdb_infrastructure;

public static class ApplicationHostBuilderExtensions
{
    public static THostBuilder AddInfrastructure<THostBuilder>(this THostBuilder builder)
        where THostBuilder : IHostApplicationBuilder
    {
        builder.Services.AddMemoryCache();

        builder.Services.AddPooledDbContextFactory<CacheDbContext>(configure =>
        {
            configure.UseMongoDB(builder.Configuration.GetConnectionString("igdb-cache")!, "igdb-cache");
        });

        builder.Services.AddTransient<CacheDbContext>(provider =>
            provider.GetRequiredService<IDbContextFactory<CacheDbContext>>().CreateDbContext());

        builder.Services.AddScoped(typeof(IRepository<>), typeof(IgdbRepository<>));

        return builder;
    }
}