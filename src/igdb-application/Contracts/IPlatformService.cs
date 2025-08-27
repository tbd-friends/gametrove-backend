using igdb_domain.Entities.ValueTypes;

namespace igdb_application.Contracts;

public interface IPlatformService
{
    Task<IEnumerable<Platform>> GetPlatformsAsync(string? name, CancellationToken cancellationToken);
}