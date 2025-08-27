using igdb_domain.Entities.Support;

namespace igdb_domain.Entities.ValueTypes;

public class Genre : IgdbEntityBase
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}