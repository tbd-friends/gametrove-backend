namespace TbdDevelop.GameTrove.GameApi.Infrastructure.Database.Models;

public class Platform
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Manufacturer { get; set; }
    public Guid Identifier { get; set; }
}