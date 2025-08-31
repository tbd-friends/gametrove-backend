using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace shared_kernel_infrastructure.EventBus;

public interface IEventBus
{
    Task PublishAsync<T>(T eventData) where T : class;

    IAsyncEnumerable<T> SubscribeAsync<T>(CancellationToken cancellationToken = default)
        where T : class;
}

public class ChannelEventBus : IEventBus, IDisposable
{
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly Channel<object> _channel;
    private readonly ChannelWriter<object> _writer;
    private readonly ChannelReader<object> _reader;

    public ChannelEventBus()
    {
        var options = new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.DropOldest 
        };

        _channel = Channel.CreateBounded<object>(options);
        _writer = _channel.Writer;
        _reader = _channel.Reader;
    }

    public async Task PublishAsync<T>(T eventData) where T : class
    {
        if (!_writer.TryWrite(eventData))
        {
            await _writer.WriteAsync(eventData); 
        }
    }

    public async IAsyncEnumerable<T> SubscribeAsync<T>(
        [EnumeratorCancellation] CancellationToken cancellationToken = default) where T : class
    {
        await foreach (var item in _reader.ReadAllAsync(cancellationToken))
        {
            if (item is T typedItem)
            {
                yield return typedItem;
            }
        }
    }

    public void Dispose()
    {
        _writer.Complete();
    }
}