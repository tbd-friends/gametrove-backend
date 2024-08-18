namespace igdb_api.Clients;

public class ReferenceAttribute : Attribute
{
    public Type Type { get; set; } = null!;
}