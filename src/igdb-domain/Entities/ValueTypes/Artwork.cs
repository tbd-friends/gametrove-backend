namespace igdb_domain.Entities.ValueTypes;

public class Artwork : Image
{
    public bool AlphaChannel { get; set; }
    public bool Animated { get; set; }
    public int ArtworkType { get; set; }
}