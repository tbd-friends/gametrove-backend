using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class PriceChartingHighlightConfiguration : IEntityTypeConfiguration<PriceChartingHighlight>
{
    public void Configure(EntityTypeBuilder<PriceChartingHighlight> builder)
    {
        builder.ToView("PriceChartingHighlights");

        builder.HasKey(k => k.PriceChartingId);
    }
}