namespace TbdDevelop.GameTrove.Games.Domain.Pricing;

public class Product
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public decimal? CompleteInBoxPrice { get; set; }
    public decimal? LoosePrice { get; set; }
    public decimal? NewPrice { get; set; }

    public static Product Invalid => new()
    {
        Id = 0,
        Name = string.Empty,
        CompleteInBoxPrice = null,
        LoosePrice = null,
        NewPrice = null
    };
}