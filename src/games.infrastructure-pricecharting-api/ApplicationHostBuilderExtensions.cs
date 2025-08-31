using games_application.Contracts;
using games_infrastructure_pricecharting_api.Client;
using games_infrastructure_pricecharting_api.Contracts;
using games_infrastructure_pricecharting_api.WorkerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shared_kernel_infrastructure.Extensions;

namespace games_infrastructure_pricecharting_api;

public static class ApplicationHostBuilderExtensions
{
    public static TBuilder AddPriceChartingInfrastructure<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services.Configure<PriceChartingOptions>(builder.Configuration.GetSection("PriceCharting"));

        builder.Services.AddHttpClient<PriceChartingApiClient>("PriceCharting",
            client => { client.BaseAddress = new Uri(builder.Configuration["PriceCharting:Url"] ?? string.Empty); });

        builder.Services.AddScoped<IPriceChartingApiClient, PriceChartingApiClient>();
        builder.Services.AddScoped<IPricingService, PricingService>();
        builder.Services.AddScoped<IPricingManagementService, PricingManagementService>();
        
        builder.AddChannelEventBus();

        builder.Services.AddHostedService<PricingFileMonitorService>();
        builder.Services.AddHostedService<PricingUpdateService>();

        return builder;
    }
}