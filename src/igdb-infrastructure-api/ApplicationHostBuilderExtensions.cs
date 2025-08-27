using igdb_application.Contracts;
using igdb_infrastructure_api.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace igdb_infrastructure_api;

public static class ApplicationHostBuilderExtensions
{
    public static TBuilder AddInfrastructureApi<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHttpClient<IgdbAuthClient>("igdb-auth");
        builder.Services.AddHttpClient<IgdbApiClient>("igdb",
            client => { client.BaseAddress = new Uri(builder.Configuration["igdb:url"] ?? string.Empty); });

        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<IPlatformService, PlatformService>();

        return builder;
    }
}