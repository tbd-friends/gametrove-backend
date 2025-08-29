using Ardalis.Specification;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Specifications;

public class ProfileByUserIdentifierSpec : Specification<Profile>, ISingleResultSpecification<Profile>
{
    public ProfileByUserIdentifierSpec(string identifier)
    {
        Query.Where(q => q.UserIdentifier == identifier);
    }
}