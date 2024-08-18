namespace Games.Infrastructure;

public class ResultSet<TEntity>
{
    public IEnumerable<TEntity> Results { get; set; } = null!;
    public int Starting { get; set; } = 0;
    public int PageSize { get; set; }
    public int Total { get; set; }
}