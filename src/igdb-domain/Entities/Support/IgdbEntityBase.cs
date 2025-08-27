namespace igdb_domain.Entities.Support;

public abstract class IgdbEntityBase
{
    public int Id { get; set; }
    public string Checksum { get; set; }
}