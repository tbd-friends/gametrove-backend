using games_application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Infrastructure.Contexts;
using TbdDevelop.GameTrove.Games.Infrastructure.Services;

namespace TbdDevelop.GameTrove.Games.Infrastructure;

public static class ApplicationHostBuilderExtensions
{
    public static THostBuilder AddInfrastructure<THostBuilder>(this THostBuilder builder)
        where THostBuilder : IHostApplicationBuilder
    {
        builder.Services.AddMemoryCache();

        builder.Services.AddPooledDbContextFactory<GameTrackingContext>((provider, configure) =>
        {
            var cache = provider.GetRequiredService<IMemoryCache>();

            configure.UseSqlServer(builder.Configuration.GetConnectionString("gametracking-work"))
                .UseMemoryCache(cache)
                .LogTo(Console.WriteLine);
        });

        builder.Services.AddScoped<GameTrackingContext>(provider =>
            provider.GetRequiredService<IDbContextFactory<GameTrackingContext>>().CreateDbContext());

        builder.Services.AddScoped(typeof(IRepository<>), typeof(GamesRepository<>));

        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        return builder;
    }
}