using igdb_application.Query.Games.Models;
using igdb_domain.Entities.ValueTypes;

namespace igdb_application.Query.Models;

public record PlatformDto(int Id, string Name, string AlternativeName, int Generation)
    : BasicInfoDto(Id, Name)
{
    public static PlatformDto FromPlatform(Platform platform)
    {
        return new PlatformDto(platform.Id, platform.Name, platform.AlternativeName, platform.Generation);
    }
}