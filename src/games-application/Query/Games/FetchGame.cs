using Ardalis.Result;
using games_application.Query.Games.Models;
using games_application.Query.Games.Specifications;
using Mediator;
using shared_kernel;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games;

public static class FetchGame
{
    public record Query(Guid Identifier) : IQuery<Result<GameWithCopyDetailDto>>;

    public class Handler(IRepository<Game> repository) : IQueryHandler<Query, Result<GameWithCopyDetailDto>>
    {
        public async ValueTask<Result<GameWithCopyDetailDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            var result = await repository.FirstOrDefaultAsync(
                new SingleGameWithCopyDetailSpec(query.Identifier),
                cancellationToken);

            return result is not null ? Result.Success(result) : Result.NotFound();
        }
    }
}