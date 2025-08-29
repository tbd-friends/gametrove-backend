using Ardalis.Result;
using games_application.Constants;
using games_application.Mapping;
using games_application.Query.Profiles.Models;
using games_application.Specifications;
using Mediator;
using shared_kernel_application.Contracts;
using shared_kernel;
using shared_kernel.Contracts;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Profiles;

public static class FetchUserProfile
{
    public record Query(string UserIdentifier) : IQuery<Result<ProfileDto>>;

    public class Handler(IRepository<Profile> profiles, ISecretStore secrets) : IQueryHandler<Query, Result<ProfileDto>>
    {
        public async ValueTask<Result<ProfileDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            var profile = await profiles.FirstOrDefaultAsync(new ProfileByUserIdentifierSpec(query.UserIdentifier),
                cancellationToken);

            var hasPriceChartingApiKey = await secrets.ExistsAsync(
                Patterns.PriceChartingApiKey(query.UserIdentifier));

            return profile is not null
                ? Result.Success(profile.AsDto(hasPriceChartingApiKey))
                : Result.NotFound();
        }
    }
}