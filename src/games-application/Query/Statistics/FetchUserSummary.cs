using Ardalis.Result;
using games_application.Query.Statistics.Results;
using games_application.Query.Statistics.Specifications;
using Mediator;
using shared_kernel;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Statistics;

public static class FetchUserSummary
{
    public record Query : IQuery<Result<UserSummaryResult>>;

    public class Handler(IRepository<UserSummary> repository) : IQueryHandler<Query, Result<UserSummaryResult>>
    {
        public async ValueTask<Result<UserSummaryResult>> Handle(Query query, CancellationToken cancellationToken)
        {
            var summary = await repository.FirstOrDefaultAsync(new UserSummarySpec(), cancellationToken);

            return summary != null ? Result.Success(summary) : Result.NotFound();
        }
    }
}