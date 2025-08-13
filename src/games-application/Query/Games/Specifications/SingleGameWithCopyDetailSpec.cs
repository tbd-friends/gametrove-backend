using Ardalis.Specification;
using games_application.Query.Games.Models;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace games_application.Query.Games.Specifications;

public class SingleGameWithCopyDetailSpec : Specification<Game, GameWithCopyDetailDto>,
    ISingleResultSpecification<Game, GameWithCopyDetailDto>
{
    public SingleGameWithCopyDetailSpec(Guid identifier)
    {
        Query
            .Where(g => g.Identifier == identifier)
            .Include(g => g.Copies)
            .ThenInclude(c => c.PriceChartingAssociation)
            .Include(g => g.Publisher)
            .Include(g => g.Platform)
            .Select(g => new GameWithCopyDetailDto
            {
                Id = g.Id,
                Name = g.Name,
                Identifier = g.Identifier,
                Platform = g.Platform.AsDto(),
                Publisher = g.Publisher != null ? g.Publisher.AsDto() : null,
                CopyCount = g.Copies.Count,
                Copies = from cp in g.Copies
                    let pc = cp.PriceChartingAssociation
                    select new GameCopyDto
                    {
                        Identifier = cp.Identifier,
                        Name = pc != null ? pc.Name : cp.Game.Name,
                        PurchasedDate = cp.PurchaseDate,
                        Cost = cp.Cost,
                        Upc = cp.Upc,
                        EstimatedValue = pc != null
                            ? cp.IsNew ? pc.NewPrice :
                            cp.IsCompleteInBox ? pc.CompleteInBoxPrice :
                            cp.IsLoose ? pc.LoosePrice : 0
                            : null,
                        Condition = cp.IsNew ? "New" : cp.IsCompleteInBox ? "Complete" : cp.IsLoose ? "Loose" : null,
                        UpdatedDate = cp.UpdatedDate
                    }
            });
    }
}