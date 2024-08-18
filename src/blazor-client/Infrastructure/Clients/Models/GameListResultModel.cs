namespace Client.Infrastructure.Clients.Models;

public class GameListResultModel
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public ItemDescriptor Platform { get; set; } = null!;
    public ItemDescriptor? Publisher { get; set; } = null!;
    public int Copies { get; set; }
}