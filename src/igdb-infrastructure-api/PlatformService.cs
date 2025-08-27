using igdb_application.Contracts;
using igdb_domain.Entities.ValueTypes;
using igdb_infrastructure_api.Client;

namespace igdb_infrastructure_api;

public class PlatformService(IgdbApiClient client) : IPlatformService
{
    public async Task<IEnumerable<Platform>> GetPlatformsAsync(string? name, CancellationToken cancellationToken)
    {
        var matching = await client.Query(
            new IGDBQuery<Platform>
            {
                Endpoint = Endpoint.Platforms,
                Where = name != null ? IgdbLanguage.Where($"platforms.name=*\"{name}\"*") : null,
                Limit = IgdbLanguage.Limit(250)
            }, cancellationToken);

        return matching ?? [];
    }
}