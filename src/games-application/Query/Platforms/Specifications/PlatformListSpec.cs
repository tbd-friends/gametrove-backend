using Ardalis.Specification;
using games_application.Query.Platforms.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Platforms.Specifications;

public sealed class PlatformListSpec : Specification<Platform, PlatformResult>
{
    public PlatformListSpec()
    {
        Query
            .Include(p => p.Mapping)
            .Select(p => new PlatformResult
            {
                Identifier = p.Identifier,
                Name = p.Name,
                Manufacturer = p.Manufacturer,
                IgdbPlatformId = p.Mapping != null ? p.Mapping.IgdbPlatformId : 0,
            });
    }
}