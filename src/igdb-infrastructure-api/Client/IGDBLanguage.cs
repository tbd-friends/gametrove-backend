namespace igdb_infrastructure_api.Client;

public class IgdbLanguage
{
    public static string Search(string query)
    {
        return $"search \"{query}\"";
    }

    public static string Where(string where)
    {
        return $"where {where}";
    }

    public static string Limit(int limit)
    {
        return $"limit {limit}";
    }
}