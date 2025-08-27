using igdb_domain.Entities.Support;

namespace igdb_domain.Entities.ValueTypes;

public class AlternativeName : IgdbEntityBase
{
    public string Comment { get; set; }
    public int Game { get; set; }
    public string Name { get; set; }
}