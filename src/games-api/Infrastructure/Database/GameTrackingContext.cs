using Microsoft.EntityFrameworkCore;
using TbdDevelop.GameTrove.GameApi.Infrastructure.Database.Models;

namespace TbdDevelop.GameTrove.GameApi.Infrastructure.Database;

public class GameTrackingContext(DbContextOptions<GameTrackingContext> options)
    : DbContext(options)
{
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<GameCopy> GameCopies { get; set; } = null!;
    public DbSet<Platform> Platforms { get; set; } = null!;
    public DbSet<PriceChartingGameCopyAssociation> PriceChartingGameCopyAssociations { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;
}