namespace igdb_infrastructure_api.Models;

public abstract class ApiResponseBase
{
    public int Id { get; set; }
    public string Checksum { get; set; }
}