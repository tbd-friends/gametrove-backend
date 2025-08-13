using Microsoft.EntityFrameworkCore;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Contexts;

public class GameTrackingContext(DbContextOptions<GameTrackingContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameTrackingContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}