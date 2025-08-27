namespace igdb_infrastructure_api.Client;

public class ReferenceAttribute : Attribute
{
    public Type Type { get; set; } = null!;
}