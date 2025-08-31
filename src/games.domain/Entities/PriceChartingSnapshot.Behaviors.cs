namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public partial class PriceChartingSnapshot
{
    public static PriceChartingSnapshot Create(
        int priceChartingId,
        string name,
        string consoleName,
        decimal? completeInBoxPrice,
        decimal? loosePrice,
        decimal? newPrice)
    {
        var snapshot = new PriceChartingSnapshot(
            priceChartingId,
            name,
            consoleName,
            completeInBoxPrice,
            loosePrice,
            newPrice);

        return snapshot;
    }

    public void UpdateSnapshot(
        string name,
        string consoleName,
        decimal completeInBoxPrice,
        decimal loosePrice,
        decimal newPrice)
    {
        if (CompleteInBoxPrice == completeInBoxPrice &&
            LoosePrice == loosePrice &&
            NewPrice == newPrice)
        {
            return;
        }

        History.Add(new PriceChartingSnapshotHistory
        {
            PriceChartingId = PriceChartingId,
            Name = Name,
            ConsoleName = ConsoleName,
            CompleteInBoxPrice = CompleteInBoxPrice,
            LoosePrice = LoosePrice,
            NewPrice = NewPrice,
            ImportDate = LastUpdated
        });

        Name = name;
        ConsoleName = consoleName;
        CompleteInBoxPrice = completeInBoxPrice;
        LoosePrice = loosePrice;
        NewPrice = newPrice;
        LastUpdated = DateTime.UtcNow;
    }

    public void InsertSnapshotHistory(
        string name,
        string consoleName,
        decimal completeInBoxPrice,
        decimal loosePrice,
        decimal newPrice,
        DateTime importDate)
    {
        if (History.All(h => h.ImportDate.Date != importDate.Date))
        {
            History.Add(new PriceChartingSnapshotHistory
            {
                PriceChartingId = PriceChartingId,
                Name = name,
                ConsoleName = consoleName,
                CompleteInBoxPrice = completeInBoxPrice,
                LoosePrice = loosePrice,
                NewPrice = newPrice,
                ImportDate = importDate
            });
        }
    }
}