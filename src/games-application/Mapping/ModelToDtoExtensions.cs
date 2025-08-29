using games_application.Dtos;
using games_application.Query.Conditions.Dtos;
using games_application.Query.Profiles.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Mapping;

public static class ModelToDtoExtensions
{
    public static PlatformDto AsDto(this Platform platform)
    {
        return new PlatformDto(platform.Identifier, platform.Name, platform.Mapping?.IgdbPlatformId);
    }

    public static PublisherDto AsDto(this Publisher publisher)
    {
        return new PublisherDto(publisher.Identifier, publisher.Name);
    }

    public static ProfileDto AsDto(this Profile profile, bool hasPriceChartingApiKey)
    {
        return new ProfileDto
        {
            Name = profile.Name,
            FavoriteGame = profile.FavoriteGame,
            HasPriceChartingApiKey = hasPriceChartingApiKey
        };
    }

    public static ConditionDto AsDto(this GameCondition condition) =>
        new() { Value = condition.Value, Name = condition.Name };
}