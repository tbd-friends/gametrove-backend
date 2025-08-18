using Ardalis.Specification;
using games_application.Query.Games.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public class PagedGamesWithDetailSpec : Specification<Game, GameDto>
{
    public PagedGamesWithDetailSpec(string? searchTerm, int start, int limit)
    {
        Query
            .Where(g => searchTerm == null || g.Name.Contains(searchTerm))
            .Include(g => g.Mapping)
            .Include(g => g.Platform)
            .ThenInclude(p => p.Mapping)
            .Include(g => g.Publisher)
            .Skip((start - 1) * limit)
            .Take(limit)
            .AsNoTracking()
            .Select(g => new GameDto
            {
                Id = g.Id,
                IgdbGameId = g.Mapping != null ? g.Mapping.IgdbGameId : null,
                Name = g.Name,
                Identifier = g.Identifier,
                Platform = g.Platform.AsDto(),
                Publisher = g.Publisher != null ? g.Publisher.AsDto() : null,
                CopyCount = g.Copies.Count
            });
    }
}