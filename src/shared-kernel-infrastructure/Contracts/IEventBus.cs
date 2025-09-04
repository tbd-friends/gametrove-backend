namespace shared_kernel_infrastructure.Contracts;

public interface IEventBus
{
    Task PublishAsync<T>(T eventData) where T : class;

    IAsyncEnumerable<T> SubscribeAsync<T>(CancellationToken cancellationToken = default)
        where T : class;
}