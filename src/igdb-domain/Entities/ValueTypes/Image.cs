using igdb_domain.Entities.Support;

namespace igdb_domain.Entities.ValueTypes;

public class Image : IgdbEntityBase
{
    public int Game { get; set; }

    public string ImageId { get; set; }
    public string Url { get; set; }

    public int Height { get; set; }
    public int Width { get; set; }
}