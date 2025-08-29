namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public partial class PriceChartingGameCopyAssociation
{
    public static PriceChartingGameCopyAssociation Create(int copyId,
        int priceChartingId,
        string name,
        string consoleName,
        decimal? completeInBoxPrice,
        decimal? loosePrice,
        decimal? newPrice)
    {
        var association = new PriceChartingGameCopyAssociation(
            copyId,
            priceChartingId,
            name,
            completeInBoxPrice,
            loosePrice,
            newPrice);
        
        association.History.Add(new PriceChartingHistory
        {
            AssociationId = association.Id,
            Name = association.Name,
            ConsoleName = consoleName,
            CompleteInBoxPrice = completeInBoxPrice,
            LoosePrice = loosePrice,
            NewPrice = newPrice,
            IsCurrent = true
        });

        return association;
    }
}