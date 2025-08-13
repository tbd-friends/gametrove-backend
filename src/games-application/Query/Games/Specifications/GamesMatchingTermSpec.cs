using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public class GamesMatchingTermSpec : Specification<Game>
{
    public GamesMatchingTermSpec(string? searchTerm)
    {
        Query
            .Where(g => searchTerm == null || g.Name.Contains(searchTerm))
            .Include(g => g.Platform)
            .Include(g => g.Publisher);
    }
}