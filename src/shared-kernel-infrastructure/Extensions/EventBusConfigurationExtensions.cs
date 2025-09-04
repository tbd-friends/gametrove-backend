using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shared_kernel_infrastructure.Contracts;
using shared_kernel_infrastructure.EventBus;

namespace shared_kernel_infrastructure.Extensions;

public static class EventBusConfigurationExtensions
{
    public static TBuilder AddChannelEventBus<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddSingleton<IEventBus, ChannelEventBus>();

        return builder;
    }
}