using games_application.Query.Conditions.Dtos;
using games_application.Query.PriceCharting.Models;
using games_application.Query.Profiles.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;
using TbdDevelop.GameTrove.Games.Domain.Pricing;

namespace games_application.SharedDtos;

public static class ModelToDtoExtensions
{
    public static PricingDto AsDto(this Product product)
    {
        return new PricingDto(product.Id, 
            product.Name, 
            product.CompleteInBoxPrice, 
            product.LoosePrice,
            product.NewPrice);
    }

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