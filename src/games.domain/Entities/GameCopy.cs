namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class GameCopy
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal? Cost { get; set; }
    public int Condition { get; set; }
    public string? Upc { get; set; }
    public DateTime UpdatedDate { get; set; }
    public Guid Identifier { get; set; }

    public bool IsNew => (Condition & 256) == 256;

    public bool IsCompleteInBox => (Condition & 32) == 32 && (Condition & 256) == 0;

    public bool IsLoose => (Condition & 128) == 128 && (Condition & 32) == 0;

    public virtual Game Game { get; set; } = null!;
    public virtual PriceChartingGameCopyAssociation? PriceChartingAssociation { get; set; }
}