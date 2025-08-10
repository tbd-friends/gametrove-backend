using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.GameApi.Infrastructure.Database.Models;

namespace TbdDevelop.GameTrove.GameApi.Infrastructure.Database.Configuration;

public class GameCopyConfiguration : IEntityTypeConfiguration<GameCopy>
{
    public void Configure(EntityTypeBuilder<GameCopy> builder)
    {
        builder.Ignore(b => b.IsNew);
        builder.Ignore(b => b.IsCompleteInBox);
        builder.Ignore(b => b.IsLoose);
    }
}