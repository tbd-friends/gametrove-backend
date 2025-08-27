using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shared_kernel.Extensions;

namespace igdb_application;

public static class ApplicationHostBuilderExtensions
{
    public static TBuilder AddApplication<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddMediator(options => { options.ServiceLifetime = ServiceLifetime.Scoped; });

        builder.AddMediatorFluentValidation(configure =>
        {
            configure.UseFluentValidation();
        });

        return builder;
    }
}