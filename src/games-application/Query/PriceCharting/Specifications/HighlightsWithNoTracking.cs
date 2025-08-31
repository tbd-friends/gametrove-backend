using Ardalis.Specification;
using games_application.Query.PriceCharting.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.PriceCharting.Specifications;

public sealed class HighlightsWithNoTracking : Specification<PriceChartingHighlight, PriceChartingHighlightDto>
{
    public HighlightsWithNoTracking(int limit = 10)
    {
        Query
            .AsNoTracking()
            .OrderByDescending(g => g.DifferencePercentage)
            .Take(limit)
            .Select(h => new PriceChartingHighlightDto
            {
                GameIdentifier = h.GameIdentifier,
                Name = h.Name,
                DifferencePercentage = h.DifferencePercentage
            });
    }
}