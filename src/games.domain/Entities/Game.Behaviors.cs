namespace TbdDevelop.GameTrove.Games.Domain.Entities;

public partial class Game
{
    public static Game Create(string name, int platformId)
    {
        var game = new Game(name, platformId);

        return game;
    }

    public void AssociateWithIgdb(int igdbGameId)
    {
        if (Mapping != null &&
            Mapping.IgdbGameId != igdbGameId)
        {
            Mapping.IgdbGameId = igdbGameId;
        }
        else
        {
            Mapping = new IgdbGameMapping
            {
                GameId = Id,
                IgdbGameId = igdbGameId
            };
        }
    }
}