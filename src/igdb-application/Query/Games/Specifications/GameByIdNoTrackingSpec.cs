using Ardalis.Specification;
using igdb_domain.Entities;

namespace igdb_application.Query.Games.Specifications;

public class GameByIdNoTrackingSpec : Specification<Game>, ISingleResultSpecification<Game>
{
    public GameByIdNoTrackingSpec(int id)
    {
        Query.Where(g => g.Id == id)
            .AsNoTracking();
    }
}