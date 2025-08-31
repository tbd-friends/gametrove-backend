using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class PriceChartingStatisticConfiguration : IEntityTypeConfiguration<PriceChartingStatistic>
{
    public void Configure(EntityTypeBuilder<PriceChartingStatistic> builder)
    {
        builder.ToView("PriceChartingStatistics");

        builder.Property(p => p.CompleteInBoxPercentageChange)
            .HasColumnName("CompleteInBoxPC");

        builder.Property(p => p.CompleteInBoxPercentageChange12Months)
            .HasColumnName("CompleteInBoxPC_12mo");

        builder.Property(p => p.LoosePercentageChange)
            .HasColumnName("LoosePC");

        builder.Property(p => p.LoosePercentageChange12Months)
            .HasColumnName("LoosePC_12mo");

        builder.Property(p => p.NewPercentageChange)
            .HasColumnName("NewPC");

        builder.Property(p => p.NewPercentageChange12Months)
            .HasColumnName("NewPC_12mo");
    }
}