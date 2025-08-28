using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Specifications;

public class GameByIdentifierSpec : Specification<Game>, ISingleResultSpecification<Game>
{
    public GameByIdentifierSpec(Guid identifier)
    {
        Query.Where(r => r.Identifier == identifier);
    }
}