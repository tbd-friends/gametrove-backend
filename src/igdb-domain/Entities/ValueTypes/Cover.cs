namespace igdb_domain.Entities.ValueTypes;

public class Cover : Image
{
    public bool AlphaChannel { get; set; }
    public bool Animated { get; set; }
}