using Ardalis.Specification;
using games_application.SharedDtos;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Publishers.Specifications;

public sealed class PublisherListSpec : Specification<Publisher, PublisherDto>
{
    public PublisherListSpec()
    {
        Query
            .Select(p => p.AsDto());
    }
}