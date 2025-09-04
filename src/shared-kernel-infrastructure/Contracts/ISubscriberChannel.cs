namespace shared_kernel_infrastructure.Contracts;

internal interface ISubscriberChannel
{
    bool IsCompleted { get; }
    Task? TryWriteAsync(object item);
    IAsyncEnumerable<object> ReadAllAsync(CancellationToken cancellationToken);
    void Complete();
}