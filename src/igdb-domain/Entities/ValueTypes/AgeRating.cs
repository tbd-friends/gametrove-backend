using igdb_domain.Entities.Support;

namespace igdb_domain.Entities.ValueTypes;

public class AgeRating
{
    public BasicEntityInfo Organization { get; set; } = null!;
    public RatingCategory RatingCategory { get; set; } = null!;
    public string? RatingCoverUrl { get; set; }
    public string? Synopsis { get; set; }
}