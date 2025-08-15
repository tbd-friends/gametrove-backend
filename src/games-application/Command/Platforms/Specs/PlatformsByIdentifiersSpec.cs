using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Platforms.Specs;

public sealed class PlatformsByIdentifiersSpec : Specification<Platform>
{
    public PlatformsByIdentifiersSpec(IEnumerable<Guid> identifiers)
    {
        Query.Include(m => m.Mapping)
            .Where(p => identifiers.Contains(p.Identifier));
    }
}