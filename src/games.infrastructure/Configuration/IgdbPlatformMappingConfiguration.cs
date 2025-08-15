using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class IgdbPlatformMappingConfiguration : IEntityTypeConfiguration<IgdbPlatformMapping>
{
    public void Configure(EntityTypeBuilder<IgdbPlatformMapping> builder)
    {
        builder.ToTable("IgdbPlatformMappings");

        builder.HasOne(k => k.Platform)
            .WithOne(p => p.Mapping)
            .HasPrincipalKey<IgdbPlatformMapping>(k => k.PlatformId);
    }
}