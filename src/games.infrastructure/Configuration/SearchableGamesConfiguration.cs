using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TbdDevelop.GameTrove.Games.Domain.Entities;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Configuration;

public class SearchableGamesConfiguration : IEntityTypeConfiguration<SearchableGame>
{
    public void Configure(EntityTypeBuilder<SearchableGame> builder)
    {
        builder.ToView("SearchableGames");
    }
}