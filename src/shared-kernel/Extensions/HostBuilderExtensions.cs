using Microsoft.Extensions.Hosting;
using shared_kernel.Validation;

namespace shared_kernel.Extensions;

public static class HostBuilderExtensions
{
    public static TBuilder AddMediatorFluentValidation<TBuilder>(this TBuilder builder, Action<ValidationBuilder> configure )
        where TBuilder : IHostApplicationBuilder
    {
        var validationBuilder = new ValidationBuilder(builder.Services);
        
        configure(validationBuilder);

        return builder;
    }
}