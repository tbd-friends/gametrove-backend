using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace shared_kernel.Validation;

public class ValidationBuilder(IServiceCollection serviceCollection)
{
    public ValidationBuilder UseFluentValidation()
    {
        serviceCollection.AddSingleton(typeof(IPipelineBehavior<,>), typeof(FluentValidationCommandPreProcessor<,>));

        return this;
    }

    public ValidationBuilder AddValidator<TValidator, TImplementation>()
        where TValidator : IValidator where TImplementation : TValidator
    {
        serviceCollection.AddTransient(typeof(TValidator), typeof(TImplementation));

        return this;
    }
}