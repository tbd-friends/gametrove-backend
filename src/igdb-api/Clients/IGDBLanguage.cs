using System.Linq.Expressions;

namespace igdb_api.Clients;

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