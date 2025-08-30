using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.PriceCharting.Specifications;

public class
    GameCopyWithAssociationsNoTrackingSpec : Specification<GameCopy>
{
    public GameCopyWithAssociationsNoTrackingSpec(Guid identifier)
    {
        Query
            .Include(c => c.PriceChartingAssociation)
            .ThenInclude(c => c.History)
            .Include(g => g.Game)
            .Where(g => g.Game.Identifier == identifier)
            .AsNoTracking();
    }
}