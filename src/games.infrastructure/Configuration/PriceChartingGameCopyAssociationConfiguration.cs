using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class PriceChartingGameCopyAssociationConfiguration : IEntityTypeConfiguration<PriceChartingGameCopyAssociation>
{
    public void Configure(EntityTypeBuilder<PriceChartingGameCopyAssociation> builder)
    {
        builder.ToTable("PriceChartingGameCopyAssociations");

        builder.HasMany(e => e.History)
            .WithOne(e => e.Association)
            .HasForeignKey(e => e.AssociationId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}