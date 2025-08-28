using Ardalis.Result;
using games_application.Query.Conditions.Dtos;
using Mediator;
using shared_kernel;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Conditions;

public static class FetchConditions
{
    public record Query : IQuery<Result<IEnumerable<ConditionDto>>>;

    public class Handler(IRepository<GameCondition> conditions)
        : IQueryHandler<Query, Result<IEnumerable<ConditionDto>>>
    {
        public async ValueTask<Result<IEnumerable<ConditionDto>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            var results = await conditions.ListAsync(cancellationToken);

            return Result.Success(results.Select(ConditionDto.FromGameCondition));
        }
    }
}