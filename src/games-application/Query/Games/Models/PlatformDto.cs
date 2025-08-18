namespace games_application.Query.Games.Models;

public sealed record PlatformDto(Guid Identifier, string Name, int? IgdbPlatformId);