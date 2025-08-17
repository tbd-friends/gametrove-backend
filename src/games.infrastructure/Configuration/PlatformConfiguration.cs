using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.ToTable("Platforms");

        builder.HasOne(p => p.Mapping)
            .WithOne(m => m.Platform)
            .HasForeignKey<IgdbPlatformMapping>(p => p.PlatformId)
            .HasPrincipalKey<Platform>(p => p.Id)
            .HasConstraintName(null)
            .OnDelete(DeleteBehavior.NoAction);
    }
}