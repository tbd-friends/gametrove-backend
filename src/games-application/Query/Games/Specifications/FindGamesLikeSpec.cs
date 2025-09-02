using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public class FindGamesLikeSpec : Specification<SearchableGame>
{
    public FindGamesLikeSpec(string search)
    {
        Query.Where(q => q.SoundexName == search);
    }
}