namespace games_application.Query.Games.Models;

public record GameWithCopyDetailDto : GameDto    
{
    public IEnumerable<GameCopyDto> Copies { get; set; }   
}