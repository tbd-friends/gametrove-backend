using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Specifications;

public class GameByTitleAndPlatformIdentifierSpec : Specification<Game>
{
    public GameByTitleAndPlatformIdentifierSpec(string title, Guid platformIdentifier)
    {
        Query
            .Include(x => x.Platform)
            .Where(x => x.Name == title && x.Platform.Identifier == platformIdentifier);
    }
}