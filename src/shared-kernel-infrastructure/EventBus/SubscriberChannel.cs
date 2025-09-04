using System.Threading.Channels;
using shared_kernel_infrastructure.Contracts;

namespace shared_kernel_infrastructure.EventBus;

internal class SubscriberChannel(Channel<object> channel)
    : ISubscriberChannel
{
    private readonly Channel<object> _channel = channel;
    private readonly ChannelWriter<object> _writer = channel.Writer;
    private readonly ChannelReader<object> _reader = channel.Reader;

    public bool IsCompleted { get; private set; }

    public Task? TryWriteAsync(object item)
    {
        try
        {
            return _writer.TryWrite(item) ? null : _writer.WriteAsync(item).AsTask();
        }
        catch (InvalidOperationException)
        {
            // Channel is closed
            return null;
        }
    }

    public IAsyncEnumerable<object> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _reader.ReadAllAsync(cancellationToken);
    }

    public void Complete()
    {
        _writer.TryComplete();

        IsCompleted = true;
    }
}