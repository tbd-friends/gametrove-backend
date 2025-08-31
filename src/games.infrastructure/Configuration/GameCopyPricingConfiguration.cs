using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class GameCopyPricingConfiguration : IEntityTypeConfiguration<GameCopyPricing>
{
    public void Configure(EntityTypeBuilder<GameCopyPricing> builder)
    {
        builder.ToTable("GameCopyPricing");

        builder.HasOne(c => c.GameCopy)
            .WithOne(c => c.Price);

        builder.HasOne(c => c.Pricing)
            .WithMany()
            .HasForeignKey(c => c.PriceChartingId)
            .HasPrincipalKey(c => c.PriceChartingId);
    }
}