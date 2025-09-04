using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using games_infrastructure_pricecharting_api.PricingUpdate.Parsers;
using games_infrastructure_pricecharting_api.WorkerServices.Events;
using games_infrastructure_pricecharting_api.WorkerServices.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using shared_kernel_infrastructure.Contracts;
using shared_kernel_infrastructure.EventBus;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_infrastructure_pricecharting_api.WorkerServices;

public class PricingFileMonitorService(
    IServiceScopeFactory factory,
    ILogger<PricingFileMonitorService> logger)
    : BackgroundService
{
    private FileSystemWatcher _watcher = null!;
    private IEventBus _eventBus = null!;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = factory.CreateAsyncScope();

        var options = scope.ServiceProvider.GetRequiredService<IOptions<PriceChartingOptions>>();

        _eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        if (!Directory.Exists(options.Value.PricingFileDirectory))
        {
            Directory.CreateDirectory(options.Value.PricingFileDirectory);
        }

        _watcher = new FileSystemWatcher(options.Value.PricingFileDirectory)
        {
            Filter = options.Value.Filter
        };
        _watcher.Created += OnFileCreated;
        _watcher.EnableRaisingEvents = true;

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        Task.Run(async () =>
        {
            await using var scope = factory.CreateAsyncScope();

            if (e.ChangeType != WatcherChangeTypes.Created)
            {
                return;
            }

            await WaitUntilFileIsAvailableAsync(e);

            await ProcessPricingFile(e, scope);
        });
    }

    private async Task ProcessPricingFile(
        FileSystemEventArgs e,
        AsyncServiceScope scope)
    {
        var sw = Stopwatch.StartNew();

        var info = new FileInfo(e.FullPath);

        var repository = scope.ServiceProvider.GetRequiredService<IRepository<PriceChartingSnapshot>>();

        var mappings = await repository.ListAsync(new MappingsUpdatedBeforeDateSpec(info.LastWriteTimeUtc),
            CancellationToken.None);

        var pricingEvents = await FetchEventsToProcess(e, info, mappings);

        foreach (var pricingEvent in pricingEvents)
        {
            logger.LogInformation("Pricing Event to Publish {PriceChartingId}", pricingEvent.PriceChartingId);

            await _eventBus.PublishAsync(pricingEvent);
        }

        sw.Stop();

        logger.LogInformation("Price File Processing took {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
    }

    private static async Task<IEnumerable<PricingUpdateEvent>> FetchEventsToProcess(
        FileSystemEventArgs e,
        FileInfo fileInfo,
        List<PriceChartingSnapshot> mappings)
    {
        await using var stream = File.OpenRead(e.FullPath);

        var pricingEvents = from x in new CsvParser<PriceRecord>(stream)
            let loosePrice = x.LoosePrice != string.Empty ? decimal.Parse(x.LoosePrice, NumberStyles.Currency) : 0
            let completePrice = x.CompletePrice != string.Empty
                ? decimal.Parse(x.CompletePrice, NumberStyles.Currency)
                : 0
            let newPrice = x.NewPrice != string.Empty ? decimal.Parse(x.NewPrice, NumberStyles.Currency) : 0
            let mapping = mappings.FirstOrDefault(m => m.PriceChartingId == x.Id)
            where mapping != null &&
                  (mapping.CompleteInBoxPrice != completePrice ||
                   mapping.LoosePrice != loosePrice ||
                   mapping.NewPrice != newPrice) &&
                  (completePrice > 0 || loosePrice > 0 || newPrice > 0)
            select new PricingUpdateEvent
            {
                PriceChartingId = x.Id,
                ConsoleName = x.Console,
                Name = x.Name,
                LoosePrice = loosePrice,
                CompletePrice = completePrice,
                NewPrice = newPrice,
                UpdatedAt = fileInfo.LastWriteTimeUtc
            };

        return pricingEvents.ToList();
    }

    private static async Task WaitUntilFileIsAvailableAsync(FileSystemEventArgs e)
    {
        using CancellationTokenSource cts = new(TimeSpan.FromMinutes(5));

        while (!cts.Token.IsCancellationRequested)
        {
            try
            {
                await using FileStream stream =
                    new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.None);

                Console.WriteLine($"File {e.Name} is ready.");

                break;
            }
            catch (IOException)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cts.Token);
            }
        }
    }

    private sealed class PriceRecord
    {
        public int Id { get; set; }
        [Column("console-name")] public string Console { get; set; } = null!;
        [Column("product-name")] public string Name { get; set; } = null!;
        [Column("loose-price")] public string LoosePrice { get; set; } = null!;
        [Column("cib-price")] public string CompletePrice { get; set; } = null!;
        [Column("new-price")] public string NewPrice { get; set; } = null!;
    }
}