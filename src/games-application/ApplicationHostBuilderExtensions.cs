using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace games_application;

public static class ApplicationHostBuilderExtensions
{
    public static TBuilder AddApplication<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddMediator(options => { options.ServiceLifetime = ServiceLifetime.Scoped; });

        return builder;
    }
}