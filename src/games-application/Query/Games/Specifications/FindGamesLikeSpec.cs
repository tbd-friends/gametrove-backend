using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public class FindGamesLikeSpec : Specification<SearchableGame>
{
    public FindGamesLikeSpec(Guid originalIdentifier, string search)
    {
        Query.Where(q => q.Identifier != originalIdentifier && q.SoundexName == search);
    }
}