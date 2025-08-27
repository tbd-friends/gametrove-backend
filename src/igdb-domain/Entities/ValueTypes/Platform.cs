using igdb_domain.Entities.Support;

namespace igdb_domain.Entities.ValueTypes;

public class Platform : BasicEntityInfo
{
    public string AlternativeName { get; set; } = null!;
    public int Generation { get; set; }
}