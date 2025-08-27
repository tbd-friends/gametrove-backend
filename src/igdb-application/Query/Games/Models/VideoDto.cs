namespace igdb_application.Query.Games.Models;

public record VideoDto(int Id, string Name, string VideoId) : BasicInfoDto(Id, Name);