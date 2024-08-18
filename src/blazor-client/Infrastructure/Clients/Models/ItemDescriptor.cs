namespace Client.Infrastructure.Clients.Models;

public class ItemDescriptor
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
}