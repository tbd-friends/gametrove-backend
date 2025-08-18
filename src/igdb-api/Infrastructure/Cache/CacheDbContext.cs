using Microsoft.EntityFrameworkCore;

namespace igdb_api.Infrastructure.Cache;

public class CacheDbContext(DbContextOptions<CacheDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CacheDbContext).Assembly);
    }
}