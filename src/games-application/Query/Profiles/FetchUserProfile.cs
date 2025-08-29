using Ardalis.Result;
using games_application.Contracts;
using games_application.Query.Profiles.Models;
using games_application.SharedDtos;
using games_application.Specifications;
using Mediator;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Profiles;

public static class FetchUserProfile
{
    public record Query(string UserIdentifier) : IQuery<Result<ProfileDto>>;

    public class Handler(IRepository<Profile> profiles, IPricingService pricing)
        : IQueryHandler<Query, Result<ProfileDto>>
    {
        public async ValueTask<Result<ProfileDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            var profile = await profiles.FirstOrDefaultAsync(new ProfileByUserIdentifierSpec(query.UserIdentifier),
                cancellationToken);

            return profile is not null
                ? Result.Success(profile.AsDto(await pricing.IsPricingEnabled(cancellationToken)))
                : Result.NotFound();
        }
    }
}