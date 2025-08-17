using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class IgdbGameMappingConfiguration : IEntityTypeConfiguration<IgdbGameMapping>
{
    public void Configure(EntityTypeBuilder<IgdbGameMapping> builder)
    {
        builder.ToTable("IgdbGameMappings");

        builder.HasKey(k => k.Id);
    }
}