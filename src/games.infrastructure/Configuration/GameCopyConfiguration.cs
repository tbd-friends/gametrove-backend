using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class GameCopyConfiguration : IEntityTypeConfiguration<GameCopy>
{
    public void Configure(EntityTypeBuilder<GameCopy> builder)
    {
        builder.ToTable("GameCopies");
        
        builder.Ignore(b => b.IsNew);
        builder.Ignore(b => b.IsCompleteInBox);
        builder.Ignore(b => b.IsLoose);

        builder.HasOne(c => c.Game)
            .WithMany(g => g.Copies);

        builder.HasOne(c => c.PriceChartingAssociation)
            .WithOne(a => a.GameCopy)
            .HasForeignKey<PriceChartingGameCopyAssociation>(a => a.GameCopyId);
    }
}