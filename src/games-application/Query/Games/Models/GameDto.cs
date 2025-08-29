using games_application.SharedDtos;

namespace games_application.Query.Games.Models;

public record GameDto
{
    public int Id { get; init; }
    public int? IgdbGameId { get; set; }
    public string Name { get; init; } = null!;
    public Guid Identifier { get; init; }
    public PlatformDto Platform { get; init; } = null!;
    public PublisherDto? Publisher { get; init; }
    public int CopyCount { get; set; }
    public DateTime? UpdatedDate { get; init; }
}