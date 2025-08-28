using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class GameConditionConfiguration : IEntityTypeConfiguration<GameCondition>
{
    public void Configure(EntityTypeBuilder<GameCondition> builder)
    {
        builder.ToTable("GameConditions");
        
        builder.HasKey(k => k.Value);
    }
}