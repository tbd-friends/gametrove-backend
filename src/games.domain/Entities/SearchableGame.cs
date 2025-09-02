namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class SearchableGame
{
    public int Id { get; set; }
    public Guid Identifier { get; set; }
    public required string Name { get; set; }
    public required string Platform { get; set; }
    public required string SoundexName { get; set; }
}