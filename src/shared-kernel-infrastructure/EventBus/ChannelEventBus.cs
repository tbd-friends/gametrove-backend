using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using shared_kernel_infrastructure.Contracts;

namespace shared_kernel_infrastructure.EventBus;

public class ChannelEventBus : IEventBus, IDisposable
{
    private readonly ConcurrentDictionary<Guid, ISubscriberChannel> _subscribers = new();
    private readonly SemaphoreSlim _publishLock = new(1, 1);
    private volatile bool _disposed;

    public async Task PublishAsync<T>(T eventData) where T : class
    {
        if (_disposed)
        {
            return;
        }

        await _publishLock.WaitAsync();
        
        try
        {
            var deadSubscribers = new List<Guid>();
            var publishTasks = new List<Task>();

            foreach (var kvp in _subscribers)
            {
                var subscriber = kvp.Value;

                if (subscriber.IsCompleted)
                {
                    deadSubscribers.Add(kvp.Key);
                    
                    continue;
                }

                var writeTask = subscriber.TryWriteAsync(eventData);
                
                if (writeTask != null)
                {
                    publishTasks.Add(writeTask);
                }
            }

            if (publishTasks.Count > 0)
            {
                await Task.WhenAll(publishTasks);
            }

            CleanupSubscribers(deadSubscribers);
        }
        finally
        {
            _publishLock.Release();
        }
    }

    public async IAsyncEnumerable<T> SubscribeAsync<T>(
        [EnumeratorCancellation] CancellationToken cancellationToken = default) where T : class
    {
        if (_disposed)
        {
            yield break;
        }

        var subscriberId = Guid.NewGuid();
        var subscriberChannel = CreateSubscriberChannel();

        _subscribers.TryAdd(subscriberId, subscriberChannel);

        try
        {
            await foreach (var item in subscriberChannel.ReadAllAsync(cancellationToken))
            {
                if (item is T typedItem)
                {
                    yield return typedItem;
                }
            }
        }
        finally
        {
            if (_subscribers.TryRemove(subscriberId, out var removedSubscriber))
            {
                removedSubscriber.Complete();
            }
        }
    }

    private static ISubscriberChannel CreateSubscriberChannel()
    {
        var options = new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false
        };

        var channel = Channel.CreateBounded<object>(options);
        
        return new SubscriberChannel(channel);
    }

    private void CleanupSubscribers(List<Guid> deadSubscribers)
    {
        foreach (var deadId in deadSubscribers)
        {
            if (_subscribers.TryRemove(deadId, out var deadSubscriber))
            {
                deadSubscriber.Complete();
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        
        _disposed = true;

        foreach (var subscriber in _subscribers.Values)
        {
            subscriber.Complete();
        }

        _subscribers.Clear();
        _publishLock.Dispose();
    }
}