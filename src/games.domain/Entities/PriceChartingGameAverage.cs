namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class PriceChartingGameAverage
{
    public int Id { get; set; }
    public decimal AverageCompleteInBoxDifference { get; set; }
    public decimal AverageLooseDifference { get; set; }
    public decimal AverageNewDifference { get; set; }

    public virtual Game Game { get; set; } = null!;
}