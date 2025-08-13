using Ardalis.Specification;
using games_application.Query.Statistics.Results;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Statistics.Specifications;

public sealed class UserSummarySpec : Specification<UserSummary, UserSummaryResult>,
    ISingleResultSpecification<UserSummary, UserSummaryResult>
{
    public UserSummarySpec()
    {
        Query
            .Select(s => new UserSummaryResult
            {
                PlatformsCount = s.Platforms,
                GameCount = s.Games,
                CopiesCount = s.Copies
            });
    }
}