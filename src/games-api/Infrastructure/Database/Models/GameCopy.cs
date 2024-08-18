namespace Games.Infrastructure.Database.Models;

public class GameCopy
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal? Cost { get; set; }
    public short Condition { get; set; }
    public string? Upc { get; set; }
    public DateTime UpdatedDate { get; set; }
    public Guid Identifier { get; set; }
}