using games_application.Contracts;
using games_infrastructure_pricecharting_api.Client;
using games_infrastructure_pricecharting_api.Constants;
using games_infrastructure_pricecharting_api.WorkerServices;
using Microsoft.Extensions.Options;
using shared_kernel_application.Contracts;

namespace games_infrastructure_pricecharting_api;

public class PricingManagementService(
    PriceChartingApiClient client,
    IOptions<PriceChartingOptions> options,
    ISecretStore secrets,
    ICurrentUserService user)
    : IPricingManagementService
{
    private Lazy<string> Key => new(() =>
        Helpers.PriceChartingApiKey(user.UserId ?? throw new ArgumentNullException($"User Invalid - {user}")));

    public async ValueTask BeginPriceChartingUpdate(CancellationToken cancellationToken = default)
    {
        var apiKey = await secrets.GetSecretAsync(Key.Value, cancellationToken);

        await using var stream = await client.DownloadCurrentPricingFile(apiKey, cancellationToken);

        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var finalFile = Path.Combine(options.Value.PricingFileDirectory, $"{timestamp}_pricecharting.csv");
        var tempFile = Path.Combine(options.Value.PricingFileDirectory, $"{timestamp}_pricecharting.tmp");

        await using (var file = new FileStream(
                         tempFile,
                         FileMode.CreateNew,
                         FileAccess.Write,
                         FileShare.None,
                         4096,
                         FileOptions.Asynchronous))
        {
            await stream.CopyToAsync(file, cancellationToken);

            await file.FlushAsync(cancellationToken);

            file.Close();
        }

        File.Move(tempFile, finalFile, overwrite: false);
    }
}