namespace igdb_api.Clients;

public class Endpoint
{
    public static Endpoint AlternativeNames = new("alternative_names");
    public static Endpoint Covers = new("covers");
    public static Endpoint Games = new("games");
    public static Endpoint Genres = new("genres");
    public static Endpoint Platforms = new("platforms");
    public static Endpoint Search = new("search");

    private readonly string _slug;

    private Endpoint(string slug)
    {
        _slug = slug;
    }

    public static implicit operator string(Endpoint endpoint)
    {
        return endpoint._slug;
    }

    public override string ToString()
    {
        return _slug;
    }
}