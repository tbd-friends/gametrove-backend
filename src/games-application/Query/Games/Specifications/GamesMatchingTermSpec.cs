using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public class GamesMatchingTermSpec : Specification<Game>
{
    public GamesMatchingTermSpec(string? searchTerm)
    {
        Query
            .Include(g => g.Copies)
            .Where(g => searchTerm == null || g.Name.Contains(searchTerm) ||
                        g.Copies.Any(c => c.Upc != null && c.Upc.Contains(searchTerm)))
            .Include(g => g.Platform)
            .Include(g => g.Publisher)
            .OrderBy(g => g.Name);
    }
}