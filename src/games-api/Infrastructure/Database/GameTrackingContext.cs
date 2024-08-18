using Games.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Games.Infrastructure.Database;

public class GameTrackingContext : DbContext
{
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<GameCopy> GameCopies { get; set; } = null!;
    public DbSet<Platform> Platforms { get; set; } = null!;
    public DbSet<PriceChartingGameCopyAssociation> PriceChartingGameCopyAssociations { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;

    public GameTrackingContext(DbContextOptions<GameTrackingContext> options) : base(options)
    {
    }
}