namespace Games.Infrastructure.Database.Models;

public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid Identifier { get; set; }
}