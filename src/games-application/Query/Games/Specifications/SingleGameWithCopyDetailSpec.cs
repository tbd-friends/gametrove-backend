using Ardalis.Specification;
using games_application.Query.Games.Models;
using games_application.SharedDtos;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public sealed class SingleGameWithCopyDetailSpec : Specification<Game, GameWithCopyDetailDto>,
    ISingleResultSpecification<Game, GameWithCopyDetailDto>
{
    public SingleGameWithCopyDetailSpec(Guid identifier)
    {
        Query
            .Where(g => g.Identifier == identifier)
            .Include(g => g.Copies)
            .ThenInclude(c => c.Price)
            .ThenInclude(p => p.Pricing)
            .Include(g => g.Mapping)
            .Include(g => g.Publisher)
            .Include(g => g.Platform)
            .ThenInclude(p => p.Mapping)
            .AsNoTracking()
            .Select(g => new GameWithCopyDetailDto
            {
                Id = g.Id,
                IgdbGameId = g.Mapping != null ? g.Mapping.IgdbGameId : null,
                Name = g.Name,
                Identifier = g.Identifier,
                Platform = g.Platform.AsDto(),
                Publisher = g.Publisher != null ? g.Publisher.AsDto() : null,
                CopyCount = g.Copies.Count,
                Copies = from cp in g.Copies
                    let pc = cp.Price.Pricing
                    select new GameCopyDto
                    {
                        Identifier = cp.Identifier,
                        IsPriceChartingLinked = pc != null,
                        Name = pc != null ? pc.Name : cp.Game.Name,
                        PurchasedDate = cp.PurchaseDate,
                        Cost = cp.Cost,
                        Upc = cp.Upc,
                        EstimatedValue = pc != null
                            ? cp.IsNew ? pc.NewPrice :
                            cp.IsCompleteInBox ? pc.CompleteInBoxPrice :
                            cp.IsLoose ? pc.LoosePrice : 0
                            : null,
                        Condition = cp.IsNew ? "New" :
                            cp.IsCompleteInBox ? "Complete" :
                            cp.IsLoose ? "Loose" : "Unknown",
                        UpdatedDate = cp.UpdatedDate
                    }
            });
    }
}