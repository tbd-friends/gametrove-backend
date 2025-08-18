namespace igdb_api.Infrastructure.Models;

public abstract class ApiResponseBase
{
    public int Id { get; set; }
    public string Checksum { get; set; }
}