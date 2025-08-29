using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.PriceCharting.Specifications;

public class CopyByIdentifierSpec : Specification<GameCopy>, ISingleResultSpecification<GameCopy>
{
    public CopyByIdentifierSpec(Guid identifier)
    {
        Query.Where(c => c.Identifier == identifier);
    }
}