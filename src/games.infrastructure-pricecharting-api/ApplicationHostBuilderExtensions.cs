using games_application.Contracts;
using games_infrastructure_pricecharting_api.Client;
using games_infrastructure_pricecharting_api.Contracts;
using games_infrastructure_pricecharting_api.WorkerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using shared_kernel_infrastructure.Extensions;

namespace games_infrastructure_pricecharting_api;

public static class ApplicationHostBuilderExtensions
{
    public static TBuilder AddPriceChartingInfrastructure<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services.Configure<PriceChartingOptions>(builder.Configuration.GetSection("PriceCharting"));

        builder.Services.AddHttpClient<PriceChartingApiClient>("PriceCharting",
                client =>
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    client.BaseAddress = new Uri(builder.Configuration["PriceCharting:Url"]!);
                })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler())
            .AddPolicyHandler(GetTimeoutPolicy())
            .AddPolicyHandler(GetRetryPolicy());

        builder.Services.AddScoped<IPriceChartingApiClient, PriceChartingApiClient>();
        builder.Services.AddScoped<IPricingService, PricingService>();
        builder.Services.AddScoped<IPricingManagementService, PricingManagementService>();

        builder.AddChannelEventBus();

        builder.Services.AddHostedService<PricingFileMonitorService>();
        builder.Services.AddHostedService<PricingUpdateService>();

        return builder;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMinutes(5));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} after {timespan}s");
                });
    }
}