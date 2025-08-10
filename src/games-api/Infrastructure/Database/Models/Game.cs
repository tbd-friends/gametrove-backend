namespace TbdDevelop.GameTrove.GameApi.Infrastructure.Database.Models;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int PlatformId { get; set; }
    public int? PublisherId { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid Identifier { get; set; }
}