using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Games.Specifications;

public sealed class PlatformByIdentifierNoTrackingSpec : Specification<Platform>, ISingleResultSpecification<Platform>
{
    public PlatformByIdentifierNoTrackingSpec(Guid identifier)
    {
        Query
            .Where(p => p.Identifier == identifier)
            .AsNoTracking();
    }
}