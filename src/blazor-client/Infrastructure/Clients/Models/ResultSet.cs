namespace Client.Infrastructure.Clients.Models;

public class ResultSet<TEntity>
{
    public IEnumerable<TEntity> Results { get; set; } = null!;
    public int Starting { get; set; } = 0;
    public int PageSize { get; set; }
    public int Total { get; set; }

    public static ResultSet<TEntity> Empty => new ResultSet<TEntity>
    {
        Results = Enumerable.Empty<TEntity>(),
        Starting = 0,
        PageSize = 0,
        Total = 0
    };
}