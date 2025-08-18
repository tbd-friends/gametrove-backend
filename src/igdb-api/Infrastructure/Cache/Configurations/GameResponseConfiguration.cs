using igdb_api.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace igdb_api.Infrastructure.Cache.Configurations;

public class GameResponseConfiguration : IEntityTypeConfiguration<GameResponse>
{
    public void Configure(EntityTypeBuilder<GameResponse> builder)
    {
        builder.ToCollection("games");
    }
}