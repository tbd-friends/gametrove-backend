using Ardalis.Specification;
using games_application.Query.Platforms.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Platforms.Specifications;

public sealed class PlatformListSpec : Specification<Platform, PlatformResult>
{
    public PlatformListSpec()
    {
        Query.Select(p => new PlatformResult
        {
            Identifier = p.Identifier,
            Name = p.Name,
            Manufacturer = p.Manufacturer
        });
    }
}