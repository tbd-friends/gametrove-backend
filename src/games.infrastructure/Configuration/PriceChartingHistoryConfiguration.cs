using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class PriceChartingHistoryConfiguration : IEntityTypeConfiguration<PriceChartingHistory>
{
    public void Configure(EntityTypeBuilder<PriceChartingHistory> builder)
    {
        builder.ToTable("PriceChartingHistory");
    }
}