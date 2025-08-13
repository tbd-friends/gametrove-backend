namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class Platform
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Manufacturer { get; set; }
    public Guid Identifier { get; set; }
}