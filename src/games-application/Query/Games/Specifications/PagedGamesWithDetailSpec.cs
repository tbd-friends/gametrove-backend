using Ardalis.Specification;
using games_application.Query.Games.Models;
using games_application.SharedDtos;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public class PagedGamesWithDetailSpec : Specification<Game, GameDto>
{
    public PagedGamesWithDetailSpec(string? searchTerm, int start, int limit)
    {
        Query
            .Include(g => g.Copies)
            .Where(g => searchTerm == null || g.Name.Contains(searchTerm) ||
                        g.Copies.Any(c => c.Upc != null && c.Upc.Contains(searchTerm)))
            .OrderBy(g => g.Name)
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