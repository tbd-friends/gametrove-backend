using Ardalis.Result;
using games_application.Query.Platforms.Models;
using games_application.Query.Platforms.Specifications;
using Mediator;
using shared_kernel;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Platforms;

public static class FetchAllPlatforms
{
    public record Query(string? Search) : IQuery<Result<IEnumerable<PlatformResult>>>;

    public class Handler(IRepository<Platform> repository) : IQueryHandler<Query, Result<IEnumerable<PlatformResult>>>
    {
        public async ValueTask<Result<IEnumerable<PlatformResult>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            var results = await repository.ListAsync(new PlatformListSpec(), cancellationToken);

            return Result.Success(results.AsEnumerable());
        }
    }
}