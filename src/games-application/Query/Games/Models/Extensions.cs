using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Models;

public static class Extensions
{
    public static PlatformDto AsDto(this Platform platform)
    {
        return new PlatformDto(platform.Identifier, platform.Name);
    }

    public static PublisherDto AsDto(this Publisher publisher)
    {
        return new PublisherDto(publisher.Identifier, publisher.Name);
    }
}