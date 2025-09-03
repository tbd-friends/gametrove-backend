using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class PriceChartingGameAveragesConfiguration : IEntityTypeConfiguration<PriceChartingGameAverage>
{
    public void Configure(EntityTypeBuilder<PriceChartingGameAverage> builder)
    {
        builder.ToView("PriceChartingGameAverageChanges");

        builder.Property(p => p.AverageCompleteInBoxDifference)
            .HasColumnName("AvgCompleteInBoxPriceDiff");

        builder.Property(p => p.AverageLooseDifference)
            .HasColumnName("AvgLoosePriceDiff");

        builder.Property(p => p.AverageNewDifference)
            .HasColumnName("AvgNewPriceDiff");
    }
}