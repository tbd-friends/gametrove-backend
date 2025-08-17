using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(g => g.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(g => g.Platform);
        builder.HasOne(g => g.Publisher);

        builder.HasMany(g => g.Copies)
            .WithOne(c => c.Game);

        builder.HasOne(g => g.Mapping)
            .WithOne(m => m.Game)
            .HasForeignKey<IgdbGameMapping>(m => m.GameId)
            .HasPrincipalKey<Game>(g => g.Id)
            .HasConstraintName(null)
            .OnDelete(DeleteBehavior.NoAction);
    }
}