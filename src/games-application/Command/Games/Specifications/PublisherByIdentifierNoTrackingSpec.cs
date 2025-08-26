using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Command.Games.Specifications;

public class PublisherByIdentifierNoTrackingSpec : Specification<Publisher>, ISingleResultSpecification<Publisher>
{
    public PublisherByIdentifierNoTrackingSpec(Guid identifier)
    {
        Query.Where(p => p.Identifier == identifier)
            .AsNoTracking();
    }
}