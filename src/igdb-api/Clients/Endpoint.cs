namespace igdb_api.Clients;

public class Endpoint
{
    public static Endpoint AlternativeNames = new Endpoint("alternative_names");
    public static Endpoint Covers = new Endpoint("covers");
    public static Endpoint Games = new Endpoint("games");
    public static Endpoint Genres = new Endpoint("genres");
    public static Endpoint Platforms = new Endpoint("platforms");

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