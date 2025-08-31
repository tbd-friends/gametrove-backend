using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class PriceChartingSnapshotConfiguration : IEntityTypeConfiguration<PriceChartingSnapshot>
{
    public void Configure(EntityTypeBuilder<PriceChartingSnapshot> builder)
    {
        builder.ToTable("PriceChartingSnapshot");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CompleteInBoxPrice)
            .HasColumnType("decimal(7,2)");
        builder.Property(p => p.LoosePrice)
            .HasColumnType("decimal(7,2)");
        builder.Property(p => p.NewPrice)
            .HasColumnType("decimal(7,2)");

        builder.HasMany(p => p.History)
            .WithOne()
            .HasForeignKey(k => k.PriceChartingId)
            .HasPrincipalKey(k => k.PriceChartingId);

        builder.HasOne(p => p.Statistics)
            .WithOne(p => p.PriceCharting)
            .HasForeignKey<PriceChartingStatistic>(s => s.Id)
            .HasPrincipalKey<PriceChartingSnapshot>(s => s.Id);
    }
}