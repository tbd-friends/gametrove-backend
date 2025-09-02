using Ardalis.Result;
using igdb_application.Command.Caching.Specifications;
using igdb_domain.Entities;
using Mediator;
using shared_kernel.Contracts;

namespace igdb_application.Command.Caching;

public static class EnqueueCacheRequest
{
    public record Command(int Id, string EntityType) : ICommand<Result>;

    public class Handler(IRepository<CacheQueueEntry> cache) : ICommandHandler<Command, Result>
    {
        public async ValueTask<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var exists = await cache.FirstOrDefaultAsync(new EntryAlreadyQueuedSpec(command.Id, command.EntityType),
                cancellationToken);

            if (exists is not null)
            {
                return Result.Conflict();
            }
            
            await cache.AddAsync(new CacheQueueEntry
            {
                EntityId = command.Id, EntityType = command.EntityType, Entered = DateTime.UtcNow
            }, cancellationToken);

            return Result.Success();
        }
    }
}