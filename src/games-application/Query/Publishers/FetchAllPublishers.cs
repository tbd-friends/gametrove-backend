using Ardalis.Result;
using games_application.Query.Publishers.Specifications;
using games_application.SharedDtos;
using Mediator;
using shared_kernel;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Publishers;

public static class FetchAllPublishers
{
    public record Query : IQuery<Result<IEnumerable<PublisherDto>>>;

    public class Handler(IRepository<Publisher> publishers)
        : IQueryHandler<Query, Result<IEnumerable<PublisherDto>>>
    {
        public async ValueTask<Result<IEnumerable<PublisherDto>>> Handle(Query query,
            CancellationToken cancellationToken)
        {
            var results = await publishers.ListAsync(new PublisherListSpec(), cancellationToken);

            return Result.Success(results.AsEnumerable());
        }
    }
}