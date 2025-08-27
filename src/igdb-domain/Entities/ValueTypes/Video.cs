using igdb_domain.Entities.Support;

namespace igdb_domain.Entities.ValueTypes;

public class Video : IgdbEntityBase
{
    public string Name { get; set; }
    public string VideoId { get; set; }
}