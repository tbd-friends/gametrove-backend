namespace TbdDevelop.GameTrove.GameApi.Infrastructure;

public class ResultSet<TEntity>
{
    public IEnumerable<TEntity> Data { get; set; } = null!;
    public MetaData? Meta { get; set; }

    public class MetaData
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
        public int TotalPages => Total / Limit;
        public bool HasMore { get; set; }
    }
}