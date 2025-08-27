using Ardalis.Result;
using igdb_application.Contracts;
using igdb_application.Query.Games.Models;
using igdb_application.Query.Models;
using Mediator;

namespace igdb_application.Query.Platforms;

public static class ListPlatforms
{
    public record Query(string? Name) : IQuery<Result<IEnumerable<PlatformDto>>>;

    public class Handler(IPlatformService service) : IQueryHandler<Query, Result<IEnumerable<PlatformDto>>>
    {
        public async ValueTask<Result<IEnumerable<PlatformDto>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var results = await service.GetPlatformsAsync(query.Name,  cancellationToken);

            return Result.Success(results.Select(PlatformDto.FromPlatform));
        }
    }
}