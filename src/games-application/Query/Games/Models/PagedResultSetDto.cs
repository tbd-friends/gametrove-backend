namespace games_application.Query.Games.Models;

public class PagedResultSetDto<TResult>
{
    public IEnumerable<TResult> Data { get; set; } = null!;
    public int Page { get; set; }
    public int Limit { get; set; }
    public int TotalResults { get; set; }
    public bool HasMore => Page + Limit < TotalResults;
}