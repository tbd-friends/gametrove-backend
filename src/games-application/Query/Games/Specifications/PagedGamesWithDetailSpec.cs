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
            .Include(g => g.Platform)
            .Include(g => g.Publisher)
            .Skip((start - 1) * limit)
            .Take(limit)
            .Select(g => new GameDto
            {
                Id = g.Id,
                Name = g.Name,
                Identifier = g.Identifier,
                Platform = g.Platform.AsDto(),
                Publisher = g.Publisher != null ? g.Publisher.AsDto() : null,
                CopyCount = g.Copies.Count
            });
    }
}