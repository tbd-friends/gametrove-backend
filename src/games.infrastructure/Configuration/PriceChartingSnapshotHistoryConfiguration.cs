using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class PriceChartingSnapshotHistoryConfiguration : IEntityTypeConfiguration<PriceChartingSnapshotHistory>
{
    public void Configure(EntityTypeBuilder<PriceChartingSnapshotHistory> builder)
    {
        builder.ToTable("PriceChartingSnapshotHistory");
        
        builder.Property(p => p.CompleteInBoxPrice)
            .HasColumnType("decimal(7,2)");
        builder.Property(p => p.LoosePrice)
            .HasColumnType("decimal(7,2)");
        builder.Property(p => p.NewPrice)
            .HasColumnType("decimal(7,2)");
    }
}