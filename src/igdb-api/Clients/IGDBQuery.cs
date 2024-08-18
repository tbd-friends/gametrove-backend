using System.Linq.Expressions;

namespace igdb_api.Clients;


public class IGDBQuery<TResult>
    where TResult : class
{
    public required string Endpoint { get; set; } = null!;
    public string? Query { get; init; }
    public int? Limit { get; init; }
    public Expression<Func<TResult, bool>>? Predicate { get; init; }
    public string[] Fields => EntityFields.GetFieldsFrom<TResult>().ToArray();

    public static implicit operator string(IGDBQuery<TResult> element)
    {
        var fields = element.Fields;
        var queryString = element.Query != null ? $"{element.Query};" : null;
        var limitString = element.Limit != null ? $"limit {element.Limit};" : null;
        var fieldsString = fields.Any() ? $"fields {string.Join(',', fields)};" : null;
        var predicateString = element.Predicate != null
            ? $"where {EntityFields.GetApiExpressionFromPredicate(element.Predicate)};"
            : null;

        var executeQuery =
            string.Join(' ', new[] { queryString, fieldsString, limitString, predicateString }
                .Where(a => a is not null));

        return executeQuery;
    }
}