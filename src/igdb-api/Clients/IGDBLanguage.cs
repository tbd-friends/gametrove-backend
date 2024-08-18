namespace igdb_api.Clients;

public class IgdbLanguage
{
    public static string Search(string query)
    {
        return $"search \"{query}\"";
    }
}