using System.Linq.Expressions;

namespace igdb_api.Clients;

public class IGDBQuery<TResult>
    where TResult : class
{
    public required string Endpoint { get; set; } = null!;
    public string? Search { get; init; }
    public string? Where { get; init; }
    public string? Limit { get; init; }

    public static implicit operator string(IGDBQuery<TResult> element)
    {
        var fields = EntityFields.GetFieldsFrom<TResult>().ToArray();
        var fieldsString = fields.Any() ? $"fields {string.Join(',', fields)}" : null;

        var executeQuery =
            string.Join(' ',
                new[] { element.Search, element.Where, fieldsString, element.Limit }
                    .Where(a => a is not null)
                    .Select(s => $"{s};"));

        return executeQuery;
    }
}