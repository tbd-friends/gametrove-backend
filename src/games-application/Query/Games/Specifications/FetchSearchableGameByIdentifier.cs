using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public class FetchSearchableGameByIdentifier : Specification<SearchableGame>, ISingleResultSpecification<SearchableGame>
{
    public FetchSearchableGameByIdentifier(Guid identifier)
    {
        Query.Where(g => g.Identifier == identifier);
    }
}