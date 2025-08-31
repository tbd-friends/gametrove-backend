using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class GameCopyConfiguration : IEntityTypeConfiguration<GameCopy>
{
    public void Configure(EntityTypeBuilder<GameCopy> builder)
    {
        builder.ToTable("GameCopies");

        builder.HasKey(g => g.Id);

        builder.Property(p => p.Cost)
            .HasColumnType("decimal(9,2)");

        builder.Ignore(b => b.IsNew);
        builder.Ignore(b => b.IsCompleteInBox);
        builder.Ignore(b => b.IsLoose);

        builder.HasOne(c => c.Game)
            .WithMany(g => g.Copies);

        builder.HasOne(gc => gc.Price)
            .WithOne()
            .HasForeignKey<GameCopyPricing>(gc => gc.GameCopyId)
            .HasPrincipalKey<GameCopy>(gc => gc.Id);
    }
}