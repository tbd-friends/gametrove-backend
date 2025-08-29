namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public class Profile
{
    public int Id { get; set; }
    public required string UserIdentifier { get; set; }
    public required string Name { get; set; }
    public string? FavoriteGame { get; set; }
}