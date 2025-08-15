using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Contexts;

public class GameTrackingDbContextFactory : IDesignTimeDbContextFactory<GameTrackingContext>
{
    public GameTrackingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GameTrackingContext>();

        // Use a dummy connection string for design-time - migrations don't need a real database
        optionsBuilder.UseSqlServer("Server=localhost;Database=DesignTimeOnly;Trusted_Connection=true;");

        return new GameTrackingContext(optionsBuilder.Options);
    }
}